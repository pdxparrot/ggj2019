using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using UnityEngine;
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

        [CanBeNull]
        public virtual Viewer Viewer { get; set; }

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
        [CanBeNull]
        private ActorController _behavior;

        [CanBeNull]
        public ActorController Behavior => _behavior;
#endregion

        public abstract bool IsLocalActor { get; }

        public virtual void Initialize(Guid id)
        {
            _id = id;
        }

        // TODO: would be better if we id radius (x) and height (y) separately
        public bool Collides(Actor other, float distance=float.Epsilon)
        {
            Vector3 offset = other.transform.position - transform.position;
            float r = other.Radius + Radius;
            float d = r * r;
            return offset.sqrMagnitude < d;
        }

#region Callbacks
        public virtual void OnSpawn(SpawnPoint spawnpoint)
        {
        }

        public virtual void OnReSpawn(SpawnPoint spawnpoint)
        {
        }

        public virtual void OnDeSpawn()
        {
        }
#endregion
    }
}
