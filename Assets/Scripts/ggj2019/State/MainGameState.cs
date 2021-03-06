﻿using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.ggj2019.Home;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class MainGameState : Game.State.MainGameState
    {
#region Effects
        [SerializeField]
        private EffectTrigger _startGameEffect;
#endregion

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
            // need to init the viewer before we start spawning players
            // so that they have a viewer to attach to
            ViewerManager.Instance.AllocateViewers(1, GameManager.Instance.GameGameData.ViewerPrefab);
            GameManager.Instance.InitViewer();

            UIManager.Instance.InitializePlayerUI(GameManager.Instance.Viewer.UICamera);

            if(!base.InitializeClient()) {
                Debug.LogWarning("Failed to initialize client!");
                return false;
            }

            // hives are built into the scene :(
            Hive[] hives = FindObjectsOfType<Hive>();
            foreach(Hive hive in hives) {
                hive.InitializeClient();
            }

            _startGameEffect.Trigger();

            return true;
        }
    }
}
