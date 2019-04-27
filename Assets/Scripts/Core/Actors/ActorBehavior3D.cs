using System;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    public class ActorBehavior3D : ActorBehavior
    {
        public Actor3D Owner3D => (Actor3D)Owner;

        public ActorMovement3D Movement3D => (ActorMovement3D)Movement;

        [Space(10)]

#region Animation
        [Header("3D Animation")]

        [SerializeField]
        [CanBeNull]
        private Animator _animator;

        [CanBeNull]
        public Animator Animator => _animator;

        [CanBeNull]
        public ActorAnimator3D ActorAnimator3D => (ActorAnimator3D)ActorAnimator;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Owner is Actor3D);
            Assert.IsTrue(Movement is ActorMovement3D);
            Assert.IsTrue(ActorAnimator == null || ActorAnimator is ActorAnimator3D);

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

            if(Animator != null) {
                Animator.enabled = !PartyParrotManager.Instance.IsPaused;
            }
        }
#endregion
    }
}
