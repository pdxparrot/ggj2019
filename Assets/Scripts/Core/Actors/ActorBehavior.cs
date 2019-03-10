using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorBehavior : MonoBehaviour
    {
        [SerializeField]
        private ActorBehaviorData _behaviorData;

        public ActorBehaviorData BehaviorData => _behaviorData;

        [SerializeField]
        private Actor _owner;

        public Actor Owner => _owner;

#region Movement
        [Header("Movement")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastMoveAxes;

        public Vector3 LastMoveAxes
        {
            get => _lastMoveAxes;
            set => _lastMoveAxes = value;
        }

        [SerializeField]
        [ReadOnly]
        protected bool _isMoving;

        public bool IsMoving => _isMoving;

        public abstract bool CanMove { get; }
#endregion

#region Physics
        public virtual Vector3 Position
        {
            get => Owner.transform.position;
            set
            {
                Debug.Log($"Teleporting actor {Owner.Id} to {value}");
                Owner.transform.position = value;
            }
        }

        public virtual Quaternion Rotation3D
        {
            get => Owner.transform.rotation;
            set => Owner.transform.rotation = value;
        }

        public virtual float Rotation2D
        {
            get => 0.0f;
            set {}
        }

        public virtual Vector3 Velocity
        {
            get => Vector3.zero;
            set {}
        }

        public virtual Vector3 AngularVelocity3D
        {
            get => Vector3.zero;
            set {}
        }

        public virtual float AngularVelocity2D
        {
            get => 0.0f;
            set {}
        }

        public virtual float Mass
        {
            get => 1.0f;
            set {}
        }

        public virtual float LinearDrag
        {
            get => 0.0f;
            set {}
        }

        public virtual float AngularDrag
        {
            get => 0.0f;
            set {}
        }

        public virtual bool IsKinematic
        {
            get => true;
            set {}
        }

        public virtual bool UseGravity
        {
            get => false;
            set {}
        }
#endregion

#region Unity Lifecycle
        protected virtual void Update()
        {
            _isMoving = LastMoveAxes.sqrMagnitude > BehaviorData.MoveAxesDeadzone;

            float dt = UnityEngine.Time.deltaTime;

            AnimationMove(LastMoveAxes, dt);
        }

        protected virtual void FixedUpdate()
        {
            float dt = UnityEngine.Time.fixedDeltaTime;

            PhysicsMove(LastMoveAxes, dt);
        }
#endregion

        public virtual void Initialize()
        {
            ResetFromData();

            LastMoveAxes = Vector3.zero;
            _isMoving = false;

            ResetPosition();

            Rotation3D = Quaternion.identity;;
            Rotation2D = 0.0f;
            Velocity = Vector3.zero;
            AngularVelocity3D = Vector3.zero;
            AngularVelocity2D = 0.0f;
        }

        // this should reset the position to (0, 0, 0)
        // without logging a teleport message
        protected abstract void ResetPosition();

        protected virtual void ResetFromData()
        {
            Mass = BehaviorData.Mass;
            LinearDrag = BehaviorData.Drag;
            AngularDrag = BehaviorData.AngularDrag;
            IsKinematic = BehaviorData.IsKinematic;
            UseGravity = !BehaviorData.IsKinematic;
        }

#region Movement
        // NOTE: axes are (x, y, 0)
        public virtual void AnimationMove(Vector3 axes, float dt)
        {
        }

        // NOTE: axes are (x, y, 0)
        public virtual void PhysicsMove(Vector3 axes, float dt)
        {
        }
#endregion
    }
}
