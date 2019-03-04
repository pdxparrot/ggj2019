using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorBehavior : MonoBehaviour
    {
        public const float AxesDeadZone = 0.001f;

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

        public virtual bool CanMove => null == _actorAnimator || !_actorAnimator.IsAnimating;
#endregion

#region Animation
#if USE_SPINE
        [SerializeField]
        [CanBeNull]
        private SpineAnimationHelper _animationHelper;

        [CanBeNull]
        public SpineAnimationHelper AnimationHelper => _animationHelper;
#else
        [SerializeField]
        [CanBeNull]
        private Animator _animator;

        [CanBeNull]
        public Animator Animator => _animator;
#endif

        [SerializeField]
        [CanBeNull]
        private ActorAnimator _actorAnimator;

        [CanBeNull]
        public ActorAnimator ActorAnimator => _actorAnimator;

        [SerializeField]
        private bool _pauseAnimationOnPause = true;
#endregion

        [SerializeField]
        private Actor _owner;

        public Actor Owner => _owner;

#region Physics
        public virtual Vector3 Position
        {
            get => transform.position;
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
        // NOTE: axes are (x, y, 0)
        public virtual void AnimationMove(Vector3 axes, float dt)
        {
        }

        // NOTE: axes are (x, y, 0)
        public virtual void PhysicsMove(Vector3 axes, float dt)
        {
        }
#endregion

#region Event Handlers
        protected virtual void PauseEventHandler(object sender, EventArgs args)
        {
            if(_pauseAnimationOnPause) {
#if USE_SPINE
                if(AnimationHelper != null) {
                    AnimationHelper.Pause(PartyParrotManager.Instance.IsPaused);
                }
#else
                if(Animator != null) {
                    Animator.enabled = !PartyParrotManager.Instance.IsPaused;
                }
#endif
            }
        }
#endregion
    }
}
