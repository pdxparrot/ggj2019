using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(ActorController2D))]
    public class ActorAnimator2D : ActorAnimator
    {
        [Serializable]
        private struct AnimationState
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

        [SerializeField]
        [ReadOnly]
        private AnimationState _animationState;

        public override bool IsAnimating => _animationState.IsAnimating;

        private ActorController2D Behavior2D => (ActorController2D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is ActorController2D);
        }
#endregion

        public override void StartAnimation2D(Vector3 targetPosition, float targetRotation, float timeSeconds, Action onComplete=null)
        {
            if(IsAnimating) {
                return;
            }

            Debug.Log($"Starting manual animation from {Behavior2D.Position}:{Behavior2D.Rotation2D} to {targetPosition}:{targetRotation} over {timeSeconds} seconds");

            _animationState.IsAnimating = true;

            _animationState.StartPosition = Behavior2D.Position;
            _animationState.EndPosition = targetPosition;

            _animationState.StartRotation = Behavior2D.Rotation2D;
            _animationState.EndRotation = targetRotation;

            _animationState.AnimationSeconds = timeSeconds;
            _animationState.AnimationSecondsRemaining = timeSeconds;

            _animationState.IsKinematic = Behavior2D.IsKinematic;
            Behavior2D.IsKinematic = true;

            _animationState.OnComplete = onComplete;
        }

        public override void StartAnimation3D(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null)
        {
            Debug.Assert(false, "ActorAnimator2D does not support 3D animations");
        }

        public override void UpdateAnimation(float dt)
        {
            if(!IsAnimating || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Profiler.BeginSample("ActorAnimator2D.UpdateAnimation");
            try {
                if(_animationState.IsFinished) {
                    //Debug.Log("Actor2D animation complete!");

                    _animationState.IsAnimating = false;

                    Behavior2D.Position = _animationState.EndPosition;
                    Behavior2D.Rotation2D = _animationState.EndRotation;
                    Behavior2D.IsKinematic = _animationState.IsKinematic;

                    _animationState.OnComplete?.Invoke();
                    _animationState.OnComplete = null;

                    return;
                }

                _animationState.AnimationSecondsRemaining -= dt;
                if(_animationState.AnimationSecondsRemaining < 0.0f) {
                    _animationState.AnimationSecondsRemaining = 0.0f;
                }

                Behavior2D.Position = Vector3.Slerp(_animationState.StartPosition, _animationState.EndPosition, _animationState.PercentComplete);
                Behavior2D.Rotation2D = Mathf.LerpAngle(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
            } finally {
                Profiler.EndSample();
            }
        }
    }
}
