using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    public class ActorBehavior2D : ActorBehavior
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;
            public float AngularVelocity;

            public void Save(Rigidbody2D rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;

                AngularVelocity = rigidbody.angularVelocity;
                rigidbody.angularVelocity = 0.0f;
            }

            public void Restore(Rigidbody2D rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
        }

        public Actor2D Owner2D => (Actor2D)Owner;

        [Space(10)]

#region Physics
        [Header("Physics")]

        private Rigidbody2D _rigidbody;

        public override Vector3 Position
        {
            get => _rigidbody.position;
            set
            {
                Debug.Log($"Teleporting actor {Owner.Id} to {value}");
                _rigidbody.position = value;
            }
        }

        public override Quaternion Rotation3D
        {
            get => Quaternion.identity;
            set {}
        }

        public override float Rotation2D
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public override Vector3 AngularVelocity3D
        {
            get => Vector3.zero;
            set {}
        }

        public override float AngularVelocity2D
        {
            get => _rigidbody.angularVelocity;
            set => _rigidbody.angularVelocity = value;
        }

        public override float Mass
        {
            get => _rigidbody.mass;
            set => _rigidbody.mass = value;
        }

        public override float LinearDrag
        {
            get => _rigidbody.drag;
            set => _rigidbody.drag = value;
        }

        public override float AngularDrag
        {
            get => _rigidbody.angularDrag;
            set => _rigidbody.angularDrag = value;
        }

        public override bool IsKinematic
        {
            get => _rigidbody.isKinematic;
            set => _rigidbody.isKinematic = value;
        }

        public override bool UseGravity
        {
            get => _rigidbody.gravityScale > 0.0f;
            set => _rigidbody.gravityScale = value ? 1.0f : 0.0f;
        }
#endregion

        [Space(10)]

#region Animation
        [Header("Animation")]

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

        [Space(10)]

#region Pause State
        [Header("Pause State")]

        [SerializeField]
        [ReadOnly]
        private InternalPauseState _pauseState;

        protected InternalPauseState PauseState => _pauseState;
#endregion

        public override bool CanMove => null == _actorAnimator || !_actorAnimator.IsAnimating;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsTrue(Owner is Actor2D);

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;

            _rigidbody = Owner2D.GetComponent<Rigidbody2D>();
            Assert.IsNotNull(_rigidbody, "ActorBehavior Owner requires a Rigidbody!");
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();

            InitRigidbody(_rigidbody);
        }

        protected override void ResetPosition()
        {
            _rigidbody.position = Vector3.zero;
        }

        protected virtual void InitRigidbody(Rigidbody2D rb)
        {
        }

        public void MovePosition(Vector3 position)
        {
            //Debug.Log($"Teleporting actor {Owner.Id} to {position} (interpolated)");

            _rigidbody.MovePosition(position);
        }

        public void MoveRotation(float angle)
        {
            _rigidbody.MoveRotation(angle);
        }

        public void AddForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddRelativeForce(force, mode);
        }

        public void AddTorque(float torque, ForceMode2D mode)
        {
            _rigidbody.AddTorque(torque, mode);
        }

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
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

            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(_rigidbody);
            } else {
                _pauseState.Restore(_rigidbody);
            }
        }
#endregion

    }
}
