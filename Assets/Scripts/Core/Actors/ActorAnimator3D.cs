using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(ActorController3D))]
    public class ActorAnimator3D : ActorAnimator
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

            public Quaternion StartRotation;
            public Quaternion EndRotation;

            public bool IsKinematic;

            public Action OnComplete;
        }

        [SerializeField]
        [ReadOnly]
        private AnimationState _animationState;

        public override bool IsAnimating => _animationState.IsAnimating;

        private ActorController3D Behavior3D => (ActorController3D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is ActorController3D);
        }
#endregion

        public override void StartAnimation2D(Vector3 targetPosition, float targetRotation, float timeSeconds, Action onComplete=null)
        {
            Debug.Assert(false, "ActorAnimator3D does not support 2D animations");
        }

        public override void StartAnimation3D(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null)
        {
            if(IsAnimating) {
                return;
            }

            Debug.Log($"Starting manual animation from {Behavior3D.Position}:{Behavior3D.Rotation3D} to {targetPosition}:{targetRotation} over {timeSeconds} seconds");

            _animationState.IsAnimating = true;

            _animationState.StartPosition = Behavior3D.Position;
            _animationState.EndPosition = targetPosition;

            _animationState.StartRotation = Behavior3D.Rotation3D;
            _animationState.EndRotation = targetRotation;

            _animationState.AnimationSeconds = timeSeconds;
            _animationState.AnimationSecondsRemaining = timeSeconds;

            _animationState.IsKinematic = Behavior3D.IsKinematic;
            Behavior3D.IsKinematic = true;

            _animationState.OnComplete = onComplete;
        }

        public override void UpdateAnimation(float dt)
        {
            if(!IsAnimating || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            Profiler.BeginSample("ActorAnimator3D.UpdateAnimation");
            try {
                if(_animationState.IsFinished) {
                    //Debug.Log("Actor3D animation complete!");

                    _animationState.IsAnimating = false;

                    Behavior3D.Position = _animationState.EndPosition;
                    Behavior3D.Rotation3D = _animationState.EndRotation;
                    Behavior3D.IsKinematic = _animationState.IsKinematic;

                    _animationState.OnComplete?.Invoke();
                    _animationState.OnComplete = null;

                    return;
                }

                _animationState.AnimationSecondsRemaining -= dt;
                if(_animationState.AnimationSecondsRemaining < 0.0f) {
                    _animationState.AnimationSecondsRemaining = 0.0f;
                }

                Behavior3D.Position = Vector3.Slerp(_animationState.StartPosition, _animationState.EndPosition, _animationState.PercentComplete);
                Behavior3D.Rotation3D = Quaternion.Slerp(_animationState.StartRotation, _animationState.EndRotation, _animationState.PercentComplete);
            } finally {
                Profiler.EndSample();
            }
        }
    }
}
