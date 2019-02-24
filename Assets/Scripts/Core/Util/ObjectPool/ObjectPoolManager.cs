#pragma warning disable 0618    // disable obsolete warning for now

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Util.ObjectPool
{
    public sealed class ObjectPoolManager : SingletonBehavior<ObjectPoolManager>
    {
        private sealed class ObjectPool
        {
            public int Size { get; }

            public int Count => _pooledObjects.Count;

            public string Tag { get; }

            public bool IsNetwork { get; }

            public bool AllowExpand { get; set; } = true;

            public PooledObject Prefab { get; }

            private readonly Queue<PooledObject> _pooledObjects;

            private GameObject _container;

            public ObjectPool(GameObject parent, string tag, PooledObject prefab, int size, bool isNetwork)
            {
                Tag = tag;
                Prefab = prefab;
                Size = size;
                IsNetwork = isNetwork;

                _container = new GameObject(Tag);
                _container.transform.SetParent(parent.transform);

                _pooledObjects = new Queue<PooledObject>(Size);

                Expand(Size);
            }

            public void Destroy()
            {
                Object.Destroy(_container);
                _container = null;
            }

            [CanBeNull]
            public PooledObject GetPooledObject(Transform parent=null, bool activate=true)
            {
                if(!_pooledObjects.Any()) {
                    if(!AllowExpand) {
                        return null;
                    }

                    Debug.LogWarning($"Expanding object pool {Tag} by {Size}!");
                    Expand(Size);
                }

                // NOTE: reparent then activate to avoid hierarchy rebuild
                PooledObject pooledObject = _pooledObjects.Dequeue();
                pooledObject.transform.SetParent(parent);
                pooledObject.gameObject.SetActive(activate);

                if(IsNetwork) {
                    Network.NetworkManager.Instance.SpawnNetworkObject(pooledObject.GetComponent<NetworkBehaviour>());
                }

                return pooledObject;
            }

            public void Expand(int amount)
            {
                Assert.IsTrue(!IsNetwork || NetworkServer.active);

                if(amount <= 0) {
                    return;
                }

                for(int i=0; i<amount; ++i) {
                    PooledObject pooledObject = Instantiate(Prefab);
                    pooledObject.Tag = Tag;
                    Recycle(pooledObject);
                }
            }

            public void EnsureSize(int size)
            {
                Expand(size - Size);
            }

            public void Recycle(PooledObject pooledObject)
            {
                if(IsNetwork) {
                    Network.NetworkManager.Instance.UnSpawnNetworkObject(pooledObject.GetComponent<NetworkBehaviour>());
                }

                // NOTE: de-activate and then repart to avoid hierarchy rebuild
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.SetParent(_container.transform);

                _pooledObjects.Enqueue(pooledObject);
            }
        }

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        private GameObject _poolContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _poolContainer = new GameObject("Object Pools");

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            foreach(var kvp in _objectPools) {
                kvp.Value.Destroy();
            }
            _objectPools.Clear();

            Destroy(_poolContainer);
            _poolContainer = null;

            base.OnDestroy();
        }
#endregion

        public bool HasPool(string poolTag)
        {
            return _objectPools.ContainsKey(poolTag);
        }

        public void InitializePool(string poolTag, PooledObject prefab, int size, bool allowExpand=true)
        {
            if(null == prefab) {
                Debug.LogError("Attempt to pool non-PooledObject!");
                return;
            }

            NetworkBehaviour networkBehaviour = prefab.GetComponent<NetworkBehaviour>();
            if(null != networkBehaviour) {
                Network.NetworkManager.Instance.RegisterNetworkPrefab(networkBehaviour);

                // network pools are server-only
                if(!NetworkServer.active) {
                    return;
                }
            }

            Debug.Log($"Initializing {(null == networkBehaviour ? "local" : "network")} object pool of size {size} for {poolTag} (allowExpand={allowExpand})");

            ObjectPool objectPool = _objectPools.GetOrDefault(poolTag);
            if(null != objectPool) {
                return;
            }

            objectPool = new ObjectPool(_poolContainer, poolTag, prefab, size, null != networkBehaviour)
            {
                AllowExpand = allowExpand
            };
            _objectPools.Add(poolTag, objectPool);
        }

        public void DestroyPool(string poolTag)
        {
            ObjectPool objectPool = _objectPools.GetOrDefault(poolTag);
            if(null == objectPool) {
                return;
            }

            if(objectPool.IsNetwork) {
                Network.NetworkManager.Instance.UnregisterNetworkPrefab(objectPool.Prefab.GetComponent<NetworkBehaviour>());
            }

            _objectPools.Remove(poolTag);
            objectPool.Destroy();
        }

        public void ExpandPool(string poolTag, int amount)
        {
            ObjectPool pool = _objectPools.GetOrDefault(poolTag);
            if(null == pool) {
                Debug.LogWarning($"No pool for tag {poolTag}!");
                return;
            }
            pool.Expand(amount);
        }

        public void EnsurePoolSize(string poolTag, int size)
        {
            ObjectPool pool = _objectPools.GetOrDefault(poolTag);
            if(null == pool) {
                Debug.LogWarning($"No pool for tag {poolTag}!");
                return;
            }
            pool.EnsureSize(size);
        }

        [CanBeNull]
        public PooledObject GetPooledObject(string poolTag, Transform parent=null, bool activate=true)
        {
            ObjectPool pool = _objectPools.GetOrDefault(poolTag);
            if(null == pool) {
                Debug.LogWarning($"No pool for tag {poolTag}!");
                return null;
            }
            return pool.GetPooledObject(parent, activate);
        }

        [CanBeNull]
        public T GetPooledObject<T>(string poolTag, Transform parent=null, bool activate=true) where T: Component
        {
            PooledObject po = GetPooledObject(poolTag, parent, activate);
            if(null != po) {
                return po.GetComponent<T>();
            }
            return null;
        }

        public void Recycle(PooledObject pooledObject)
        {
            ObjectPool pool = _objectPools.GetOrDefault(pooledObject.Tag);
            if(null == pool) {
                Debug.LogWarning($"No pool for recycled object {pooledObject.name}, destroying...");
                Destroy(pooledObject.gameObject);
            } else {
                pool.Recycle(pooledObject);
            }
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.ObjectPoolManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Object Pools:", GUI.skin.box);
                    foreach(var kvp in _objectPools) {
                        GUILayout.BeginVertical(kvp.Key, GUI.skin.box);
                            GUILayout.Label($"Expandable: {kvp.Value.AllowExpand}");
                            GUILayout.Label($"Networked: {kvp.Value.IsNetwork}");
                            GUILayout.Label($"Size: {kvp.Value.Count} / {kvp.Value.Size}");
                        GUILayout.EndVertical();
                    }
                GUILayout.EndVertical();
            };
        }
    }
}
