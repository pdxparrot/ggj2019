using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private Guid _id;

        public Guid Id => _id;

        public abstract float Height { get; }

        public abstract float Radius { get; }

#region Model
        [CanBeNull]
        [SerializeField]
        private GameObject _model;

        [CanBeNull]
        public GameObject Model
        {
            get => _model;
            protected set => _model = value;
        }
#endregion

#region Behavior
        [SerializeField]
        [FormerlySerializedAs("_controller")]
        private ActorBehavior _behavior;

        public ActorBehavior Behavior => _behavior;
#endregion

#region Network
        public abstract bool IsLocalActor { get; }
#endregion

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsNotNull(Behavior);
        }

        protected virtual void OnDestroy()
        {
            if(ActorManager.HasInstance) {
                ActorManager.Instance.Unregister(this);
            }
        }
#endregion

        public virtual void Initialize(Guid id, ActorBehaviorData behaviorData)
        {
            _id = id;
            name = Id.ToString();

            Behavior.Initialize(behaviorData);
        }

        // TODO: would be better if we id radius (x) and height (y) separately
        public bool Collides(Actor other, float distance=float.Epsilon)
        {
            Vector3 offset = other.Behavior.Position - Behavior.Position;
            float r = other.Radius + Radius;
            float d = r * r;
            return offset.sqrMagnitude < d;
        }

#region Events
        public virtual bool OnSpawn(SpawnPoint spawnpoint)
        {
            ActorManager.Instance.Register(this);

            Behavior.OnSpawn(spawnpoint);

            return true;
        }

        public virtual bool OnReSpawn(SpawnPoint spawnpoint)
        {
            ActorManager.Instance.Register(this);

            Behavior.OnReSpawn(spawnpoint);

            return true;
        }

        public virtual void OnDeSpawn()
        {
            Behavior.OnDeSpawn();

            if(ActorManager.HasInstance) {
                ActorManager.Instance.Unregister(this);
            }
        }
#endregion
    }
}
