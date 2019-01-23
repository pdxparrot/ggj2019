using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActorController2D : ActorController
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;

            public void Save(Rigidbody2D rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody2D rigidbody)
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

            public float StartRotation;
            public float EndRotation;

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
        private float _lastAngularVelocity;

        public PhysicsActor2D PhysicsOwner => (PhysicsActor2D)Owner;

        public Rigidbody2D Rigidbody { get; private set; }

        public override Vector3 Position => Rigidbody.position;

        public override Quaternion Rotation3D
        {
            get => new Quaternion();
            set {}
        }

        public override float Rotation2D
        {
            get => Rigidbody.rotation;
            set => Rigidbody.rotation = value;
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

            Assert.IsTrue(Owner is PhysicsActor2D);

            Rigidbody = GetComponent<Rigidbody2D>();
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
            Rigidbody.AddForce(force, ForceMode2D.Force);
        }

        public void StartAnimation2D(Vector3 targetPosition, float targetRotation, float timeSeconds, Action onComplete)
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

        public void StartAnimation3D(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null)
        {
            Debug.Assert(false, "ActorController2D does not support 3D animations");
        }

        protected override void UpdateAnimation(float dt)
        {
            if(!IsAnimating || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Profiler.BeginSample("ActorController2D.UpdateAnimation");
            try {
                if(_animationState.IsFinished) {
                    //Debug.Log("Manual 2D animation complete!");

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
                Rigidbody.rotation = Mathf.LerpAngle(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
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
