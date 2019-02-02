using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string _tag;

        [SerializeField]
        private Actor _overridePrefab;

        public string Tag => _tag;

        public Actor OverridePrefab => _overridePrefab;

        private Actor _actor = null;

#region Unity Lifecycle
        protected virtual void OnEnable()
        {
            SpawnManager.Instance.RegisterSpawnPoint(this);
        }

        protected virtual void OnDisable()
        {
            if(SpawnManager.HasInstance) {
                SpawnManager.Instance.UnregisterSpawnPoint(this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
#endregion

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
            // -- use the placed one if it's there, or the passed in one if not
            Actor p = (OverridePrefab ? OverridePrefab : prefab);

            Actor actor = Instantiate(p);
            //Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");
            Spawn(actor);
            return actor;
        }

        public virtual void Spawn(Actor actor)
        {
            _actor = actor;
            InitActor(actor);

            actor.OnSpawn(gameObject);
        }

        public virtual void ReSpawn(Actor actor)
        {
            InitActor(actor);

            actor.OnReSpawn(gameObject);
        }

        public bool Occupied {
            get {
                return _actor != null;
            }
        }
    }
}
