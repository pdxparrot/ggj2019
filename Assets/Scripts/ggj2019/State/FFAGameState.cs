﻿using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class FFAGameState : MainGameState
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
            ViewerManager.Instance.AllocateViewers(1, GameManager.Instance.GameGameData.ViewerPrefab);

            if(!base.InitializeClient()) {
                Debug.LogWarning("Failed to initialize client!");
                return false;
            }

            GameManager.Instance.InitViewer();
            UIManager.Instance.InitializePlayerUI(GameManager.Instance.Viewer.UICamera);
            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowPlayerHUD(true);

            return true;
        }
    }
}
