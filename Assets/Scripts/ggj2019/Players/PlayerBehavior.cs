using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerBehavior : Game.Players.PlayerBehavior2D
    {
        public Data.PlayerBehaviorData GamePlayerBehaviorData => (Data.PlayerBehaviorData)PlayerBehaviorData;

        public Player GamePlayer => (Player)Player;

        // start true to force the animation the first time
        // TODO: is this actually necessary?
        [SerializeField]
        [ReadOnly]
        private bool _isFlying = true;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerBehaviorData is Data.PlayerBehaviorData);
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();

            SpineAnimation.SetFacing(Vector3.zero - transform.position);
            SetIdleAnimation();
        }

        public override void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            base.DefaultAnimationMove(axes, dt);

            if(IsMoving) {
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

            SpineAnimation.SetAnimation(PlayerManager.Instance.GamePlayerData.IdleAnimationName, true);
            _isFlying = false;
        }

        private void SetFlyingAnimation()
        {
            if(_isFlying) {
                return;
            }

            SpineAnimation.SetAnimation(PlayerManager.Instance.GamePlayerData.FlyingAnimationName, true);
            _isFlying = true;
        }
#endregion
    }
}
