using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(Rigidbody))]
    public class ActorController3D : ActorController
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;

            public void Save(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
            }
        }

        [Serializable]
        private struct ManualAnimationState
        {
            public bool IsAnimating;

            public float AnimationSeconds;
            public float AnimationSecondsRemaining;

            public float PercentComplete => 1.0f - (AnimationSecondsRemaining / AnimationSeconds);

            public bool IsFinished => AnimationSecondsRemaining <= 0.0f;

            public Vector3 StartPosition;
            public Vector3 EndPosition;

            public Quaternion StartRotation;
            public Quaternion EndRotation;

            public bool IsKinematic;

            public Action OnComplete;
        }

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;

        public PhysicsActor3D PhysicsOwner => (PhysicsActor3D)Owner;

        public Rigidbody Rigidbody { get; private set; }

        public override Vector3 Position => Rigidbody.position;

        public override Quaternion Rotation3D
        {
            get => Rigidbody.rotation;
            set => Rigidbody.rotation = value;
        }

        public override float Rotation2D
        {
            get => 0;
            set {}
        }

        public override Vector3 Velocity
        {
            get => Rigidbody.velocity;
            set => Rigidbody.velocity = value;
        }

        public override float Mass
        {
            get => Rigidbody.mass;
            set => Rigidbody.mass = value;
        }

        public override float LinearDrag
        {
            get => Rigidbody.drag;
            set => Rigidbody.drag = value;
        }

        public override float AngularDrag
        {
            get => Rigidbody.angularDrag;
            set => Rigidbody.angularDrag = value;
        }
#endregion

        [Space(10)]

#region Manual Animation
        [Header("Manual Animation")]

        [SerializeField]
        [ReadOnly]
        private ManualAnimationState _animationState;

        public override bool IsAnimating => _animationState.IsAnimating;
#endregion

        [Space(10)]

#region Pause State
        [Header("Pause State")]

        [SerializeField]
        [ReadOnly]
        private InternalPauseState _pauseState;

        protected InternalPauseState PauseState => _pauseState;
#endregion

        public override bool CanMove => !IsAnimating;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Owner is PhysicsActor3D);

            Rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
            _lastAngularVelocity = Rigidbody.angularVelocity;
        }
#endregion

        public override void MoveTo(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");

            Rigidbody.position = position;
        }

        public void MovePosition(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position} (interpolated)");

            Rigidbody.MovePosition(position);
        }

        public void AddForce(Vector3 force)
        {
            Rigidbody.AddForce(force, ForceMode.Force);
        }

        public void StartAnimation3D(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null)
        {
            if(IsAnimating) {
                return;
            }

            Debug.Log($"Starting manual animation from {Rigidbody.position}:{Rigidbody.rotation} to {targetPosition}:{targetRotation} over {timeSeconds} seconds");

            _animationState.IsAnimating = true;

            _animationState.StartPosition = Rigidbody.position;
            _animationState.EndPosition = targetPosition;

            _animationState.StartRotation = Rigidbody.rotation;
            _animationState.EndRotation = targetRotation;

            _animationState.AnimationSeconds = timeSeconds;
            _animationState.AnimationSecondsRemaining = timeSeconds;

            _animationState.IsKinematic = Rigidbody.isKinematic;
            Rigidbody.isKinematic = true;

            _animationState.OnComplete = onComplete;
        }

        public void StartAnimation2D(Vector3 targetPosition, float targetRotation, float timeSeconds, Action onComplete=null)
        {
            Debug.Assert(false, "ActorController3D does not support 2D animations");
        }

        protected override void UpdateAnimation(float dt)
        {
            if(!IsAnimating || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Profiler.BeginSample("ActorController3D.UpdateAnimation");
            try {
                if(_animationState.IsFinished) {
                    //Debug.Log("Manual 3D animation complete!");

                    _animationState.IsAnimating = false;

                    Rigidbody.position = _animationState.EndPosition;
                    Rigidbody.rotation = _animationState.EndRotation;
                    Rigidbody.isKinematic = _animationState.IsKinematic;

                    _animationState.OnComplete?.Invoke();
                    _animationState.OnComplete = null;

                    return;
                }

                _animationState.AnimationSecondsRemaining -= dt;
                if(_animationState.AnimationSecondsRemaining < 0.0f) {
                    _animationState.AnimationSecondsRemaining = 0.0f;
                }

                Rigidbody.position = Vector3.Slerp(_animationState.StartPosition, _animationState.EndPosition, _animationState.PercentComplete);
                Rigidbody.rotation = Quaternion.Slerp(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
            } finally {
                Profiler.EndSample();
            }
        }

#region Event Handlers
        protected override void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(Rigidbody);
            } else {
                _pauseState.Restore(Rigidbody);
            }
        }
#endregion
    }
}
