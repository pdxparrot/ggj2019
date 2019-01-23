using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class MainGameState : Game.State.MainGameState
    {
        protected override bool InitializeServer()
        {
            if(!base.InitializeServer()) {
                Debug.LogWarning("Failed to initialize server!");
                return false;
            }

            GameManager.Instance.StartGame();

            return true;
        }

        protected override bool InitializeClient()
        {
            ViewerManager.Instance.AllocateViewers(1, (PlayerViewer)PlayerManager.Instance.PlayerData.PlayerViewerPrefab);

            if(!base.InitializeClient()) {
                Debug.LogWarning("Failed to initialize client!");
                return false;
            }

            return true;
        }
    }
}
