using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorController : MonoBehaviour
    {
        public static float AxesDeadZone = 0.001f;

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
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        public virtual bool CanMove => true;
#endregion

        public virtual bool IsAnimating => false;

        [SerializeField]
        private Actor _owner;

        public Actor Owner => _owner;

#region Physics
        public abstract Vector3 Position { get; }

        public abstract Quaternion Rotation3D { get; set; }

        public abstract float Rotation2D { get; set; }

        public abstract Vector3 Velocity { get; set; }

        public abstract float Mass { get; set; }

        public abstract float LinearDrag { get; set; }

        public abstract float AngularDrag { get; set; }
#endregion

#region Unity Lifecycle
        protected virtual void Awake()
        {
            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void Update()
        {
            _isMoving = LastMoveAxes.sqrMagnitude > AxesDeadZone;

            float dt = Time.deltaTime;

            UpdateAnimation(dt);

            AnimationMove(LastMoveAxes, dt);
        }

        protected virtual void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            PhysicsMove(LastMoveAxes, dt);
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

        public virtual void Initialize()
        {
        }

#region Movement
        public virtual void MoveTo(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");

            transform.position = position;
        }

        // NOTE: axes are (x, y, 0)
        public virtual void AnimationMove(Vector3 axes, float dt)
        {
        }

        // NOTE: axes are (x, y, 0)
        public virtual void PhysicsMove(Vector3 axes, float dt)
        {
        }
#endregion

#region Manual Animations
        protected virtual void UpdateAnimation(float dt)
        {
        }
#endregion

#region Event Handlers
        protected virtual void PauseEventHandler(object sender, EventArgs args)
        {
        }
#endregion
    }
}
