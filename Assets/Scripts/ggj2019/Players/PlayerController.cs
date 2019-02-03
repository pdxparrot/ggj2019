using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerController : Game.Players.PlayerController2D
    {
        public Data.PlayerControllerData GamePlayerControllerData => (Data.PlayerControllerData)PlayerControllerData;

        public Player GamePlayer => (Player)Player;

        [SerializeField]
        private SkeletonAnimation _animation;

        // start true to force the animation the first time
        private bool _isFlying = true;

        public override void Initialize()
        {
            base.Initialize();

            _animation.Skeleton.ScaleX = transform.position.x > 0 ? 1.0f : -1.0f;
            SetHoverAnimation();
        }

        public override void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(IsMoving) {
                SetFlightAnimation();
                _animation.Skeleton.ScaleX = LastMoveAxes.x < 0 ? -1.0f : 1.0f;
            } else  {
                SetHoverAnimation();
            }
        }

        private void SetHoverAnimation()
        {
            if(!_isFlying) {
                return;
            }

            SetAnimation("bee_hover", true);
            _isFlying = false;
        }

        private void SetFlightAnimation()
        {
            if(_isFlying) {
                return;
            }

            SetAnimation("bee-flight", true);
            _isFlying = true;
        }

        private void SetAnimation(string animationName, bool loop)
        {
            _animation.AnimationState.SetAnimation(0, animationName, loop);
        }
    }
}
