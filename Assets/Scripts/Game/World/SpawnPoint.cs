﻿using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string _tag;

        public string Tag => _tag;

        private Actor _actor = null;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            SpawnManager.Instance.RegisterSpawnPoint(this);
        }

        protected virtual void OnDestroy()
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
            Actor actor = Instantiate(prefab);
            //Debug.LogWarning("You probably meant to use NetworkManager.SpawnNetworkPrefab");
            Spawn(actor);
            return actor;
        }

        public virtual void Spawn(Actor actor)
        {
            _actor = actor;
            InitActor(actor);

            actor.OnSpawn();
        }

        public virtual void ReSpawn(Actor actor)
        {
            InitActor(actor);

            actor.OnReSpawn();
        }

        public bool Occupied {
            get {
                return _actor != null;
            }
        }
    }
}
