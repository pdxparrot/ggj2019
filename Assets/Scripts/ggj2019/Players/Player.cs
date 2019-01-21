#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(FollowTarget3D))]
    public sealed class Player : Game.Players.Player
    {
        public override float Height => PlayerController.Capsule.height;

        public override float Radius => PlayerController.Capsule.radius;

        public FollowTarget3D FollowTarget { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if(!(Controller is PlayerController)) {
                Debug.LogError("Player controller must be a PlayerController!");
            }
#endif

            FollowTarget = GetComponent<FollowTarget3D>();
        }
#endregion

        protected override bool InitializeLocalPlayer(int id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

Debug.Log("TODO: init player HUD");
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
