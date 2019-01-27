using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Collider.bounds.size.y;

        public override float Radius => GamePlayerController.Collider.bounds.size.x;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerController is PlayerController);
        }
#endregion

        protected override void InitializeViewer()
        {
            PlayerViewer = GameManager.Instance.Viewer;
        }

        protected override void InitializePlayerUI()
        {
Debug.LogWarning($"TODO: Initialize UI for player {Id}");
/*
            if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowInfoText();
            }
*/

        }

#region Actions
        public void DoGather()
        {
            Debug.LogWarning("TODO: gather the swarm");
        }

        public void DoContext()
        {
            Debug.LogWarning("TODO: do a context thing");
        }
#endregion
    }
}
