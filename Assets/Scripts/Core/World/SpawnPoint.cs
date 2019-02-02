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

#region Unity Lifecycle
        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
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
            if(SpawnManager.HasInstance) {
                SpawnManager.Instance.RegisterSpawnPoint(this);
            }
        }

        private void Unregister()
        {
            if(SpawnManager.HasInstance) {
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

        public virtual Actor SpawnPrefab(Actor prefab)
        {
            Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");

            Actor actor = Instantiate(prefab);
            Spawn(actor);
            return actor;
        }

        public virtual void Spawn(Actor actor)
        {
            InitActor(actor);

            actor.OnSpawn(this);
        }

        public virtual void ReSpawn(Actor actor)
        {
            InitActor(actor);

            actor.OnReSpawn(this);
        }

        public bool Acquire(Actor owner)
        {
            if(null != _owner) {
                return false;
            }

            _owner = owner;
            Unregister();

            return true;
        }

        public void Release()
        {
            if(null == _owner) {
                return;
            }

            _owner = null;
            Register();
        }
    }
}
