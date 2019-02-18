using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerController : Game.Players.PlayerController2D
    {
        public Data.PlayerControllerData GamePlayerControllerData => (Data.PlayerControllerData)PlayerControllerData;

        public Player GamePlayer => (Player)Player;

        [SerializeField]
        private SpineAnimationHelper _animation;

        // start true to force the animation the first time
        // TODO: is this actually necessary?
        [SerializeField]
        [ReadOnly]
        private bool _isFlying = true;

        public override void Initialize()
        {
            base.Initialize();

            _animation.SetFacing(Vector3.zero - transform.position);
            SetIdleAnimation();
        }

        public override void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(IsMoving) {
                _animation.SetFacing(LastMoveAxes);
                SetFlyingAnimation();
            } else  {
                SetIdleAnimation();
            }
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(!_isFlying) {
                return;
            }

            _animation.SetAnimation(PlayerManager.Instance.GamePlayerData.IdleAnimationName, true);
            _isFlying = false;
        }

        private void SetFlyingAnimation()
        {
            if(_isFlying) {
                return;
            }

            _animation.SetAnimation(PlayerManager.Instance.GamePlayerData.FlyingAnimationName, true);
            _isFlying = true;
        }
#endregion
    }
}
