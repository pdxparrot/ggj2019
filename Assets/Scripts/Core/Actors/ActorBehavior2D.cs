using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Animation;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    public class ActorBehavior2D : ActorBehavior
    {
        public Actor2D Owner2D => (Actor2D)Owner;

        public ActorMovement2D Movement2D => (ActorMovement2D)Movement;

        [Space(10)]

#region Animation
        [Header("2D Animation")]

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

        [CanBeNull]
        public ActorAnimator2D ActorAnimator2D => (ActorAnimator2D)ActorAnimator;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Owner is Actor2D);
            Assert.IsTrue(Movement is ActorMovement2D);
            Assert.IsTrue(ActorAnimator == null || ActorAnimator is ActorAnimator2D);

            base.Awake();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(!PauseAnimationOnPause) {
                return;
            }

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
#endregion
    }
}
