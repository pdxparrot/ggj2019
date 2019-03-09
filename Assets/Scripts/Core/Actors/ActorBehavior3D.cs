using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    public class ActorBehavior3D : ActorBehavior
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;

            public void Save(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;

                AngularVelocity = rigidbody.angularVelocity;
                rigidbody.angularVelocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
        }

        public Actor3D Owner3D => (Actor3D)Owner;

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;

        private Rigidbody _rigidbody;

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
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public override float Rotation2D
        {
            get => 0.0f;
            set {}
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public override Vector3 AngularVelocity3D
        {
            get => _rigidbody.angularVelocity;
            set => _rigidbody.angularVelocity = value;
        }

        public override float AngularVelocity2D
        {
            get => 0.0f;
            set {}
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
            get => _rigidbody.useGravity;
            set => _rigidbody.useGravity = value;
        }
#endregion

        [Space(10)]

#region Animation
        [Header("Animation")]

        [SerializeField]
        [CanBeNull]
        private Animator _animator;

        [CanBeNull]
        public Animator Animator => _animator;

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
            Assert.IsTrue(Owner is Actor3D);

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;

            _rigidbody = Owner3D.GetComponent<Rigidbody>();
            Assert.IsNotNull(_rigidbody, "ActorBehavior Owner requires a Rigidbody!");
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = _rigidbody.velocity;
            _lastAngularVelocity = _rigidbody.angularVelocity;
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();

            InitRigidbody(_rigidbody);
        }

        protected virtual void InitRigidbody(Rigidbody rb)
        {
        }

        public void MovePosition(Vector3 position)
        {
            //Debug.Log($"Teleporting actor {Owner.Id} to {position} (interpolated)");

            _rigidbody.MovePosition(position);
        }

        public void MoveRotation(Quaternion rot)
        {
            _rigidbody.MoveRotation(rot);
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddRelativeForce(force, mode);
        }

        public void AddTorque(Vector3 torque, ForceMode mode)
        {
            _rigidbody.AddTorque(torque, mode);
        }

        public void AddRelativeTorque(Vector3 torque, ForceMode mode)
        {
            _rigidbody.AddRelativeTorque(torque, mode);
        }

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(_pauseAnimationOnPause) {
                if(Animator != null) {
                    Animator.enabled = !PartyParrotManager.Instance.IsPaused;
                }
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
