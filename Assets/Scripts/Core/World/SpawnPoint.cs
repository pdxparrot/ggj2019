using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.World
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string _tag;

        public string Tag => _tag;

        [SerializeField]
        [ReadOnly]
        private Actor _owner;

        private Action _onRelease;

#region Unity Lifecycle
        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            Release();
            Unregister();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endregion

        private void Register()
        {
            if(enabled && SpawnManager.HasInstance) {
                SpawnManager.Instance.RegisterSpawnPoint(this);
            }
        }

        private void Unregister()
        {
            if(enabled && SpawnManager.HasInstance) {
                SpawnManager.Instance.UnregisterSpawnPoint(this);
            }
        }

        private void InitActor(Actor actor)
        {
            Transform actorTransform = actor.transform;
            Transform thisTransform = transform;

            actorTransform.position = thisTransform.position;
            actorTransform.rotation = thisTransform.rotation;

            actor.gameObject.SetActive(true);
        }

        private void InitActor(Actor actor, Guid id)
        {
            InitActor(actor);

            actor.Initialize(id);
        }

        public virtual Actor SpawnPrefab(Actor prefab, Guid id)
        {
            Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");

            Actor actor = Instantiate(prefab);
            Spawn(actor, id);
            return actor;
        }

        public virtual void Spawn(Actor actor, Guid id)
        {
            InitActor(actor, id);

            actor.OnSpawn(this);
        }

        public virtual void SpawnPlayer(Actor actor)
        {
            InitActor(actor);

            actor.OnSpawn(this);
        }

        public virtual void ReSpawn(Actor actor)
        {
            InitActor(actor);

            actor.OnReSpawn(this);
        }

        public bool Acquire(Actor owner, Action onRelease, bool force=false)
        {
            if(!force && null != _owner) {
                return false;
            }

            Release();

            _owner = owner;
            _onRelease = onRelease;

            Unregister();

            return true;
        }

        public void Release()
        {
            if(null == _owner) {
                return;
            }

            _onRelease?.Invoke();
            _owner = null;

            Register();
        }
    }
}
