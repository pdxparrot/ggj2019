#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(FollowTarget3D))]
    public sealed class Player : Game.Players.Player3D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Capsule.height;

        public override float Radius => GamePlayerController.Capsule.radius;

        public FollowTarget3D FollowTarget { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerController is PlayerController);

            FollowTarget = GetComponent<FollowTarget3D>();
        }
#endregion

        protected override bool InitializeLocalPlayer(int id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

Debug.LogWarning("TODO: init player HUD");
/*
            UIManager.Instance.InitializePlayerUI(this);
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowInfoText();
            }
*/

            return true;
        }
    }
}
