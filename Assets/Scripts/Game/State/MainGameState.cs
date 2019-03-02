#pragma warning disable 0618    // disable obsolete warning for now

using System;
using System.Collections.Generic;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Loading;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.State
{
    public abstract class MainGameState : GameState
    {
        [SerializeField]
        private GameOverState _gameOverState;

        [SerializeField]
        private AudioClip _music;

        private ServerSpectator _serverSpectator;

        public override IEnumerator<LoadStatus> OnEnterRoutine()
        {
            yield return new LoadStatus(0.0f, "Initializing...");

            IEnumerator<LoadStatus> runner = base.OnEnterRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(0.5f, "Initializing...");

            runner = InitializeRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
            Core.Network.NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;

            yield return new LoadStatus(1.0f, "Done!");
        }

        public override void OnUpdate(float dt)
        {
            if(GameStateManager.Instance.GameManager.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState, state => {
                    state.Initialize();
                });
            }
        }

        public override IEnumerator<LoadStatus> OnExitRoutine()
        {
            yield return new LoadStatus(0.0f, "Shutting down...");

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
            }

            IEnumerator<LoadStatus> runner = ShutdownRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(0.5f, "Shutting down...");

            runner = base.OnExitRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new LoadStatus(1.0f, "Done!");
        }

#region Initialize
        private IEnumerator<LoadStatus> InitializeRoutine()
        {
            yield return new LoadStatus(0.0f, "Initializing...");

            PartyParrotManager.Instance.IsPaused = false;

            DebugMenuManager.Instance.ResetFrameStats();

            yield return new LoadStatus(0.25f, "Initializing...");

            Assert.IsNotNull(GameStateManager.Instance.GameManager);
            GameStateManager.Instance.GameManager.Initialize();

            yield return new LoadStatus(0.75f, "Initializing...");

            InitializeServer();
            InitializeClient();

            yield return new LoadStatus(1.0f, "Initializing...");
        }

        protected virtual bool InitializeServer()
        {
            if(!NetworkServer.active) {
                return false;
            }

            Core.Network.NetworkManager.Instance.ServerChangedScene();

            if(!NetworkClient.active && !PartyParrotManager.Instance.IsHeadless) {
                ViewerManager.Instance.AllocateViewers(1, GameStateManager.Instance.ServerSpectatorViewer);

                _serverSpectator = Instantiate(GameStateManager.Instance.ServerSpectatorPrefab);
            }

            return true;
        }

        protected virtual bool InitializeClient()
        {
            if(!NetworkClient.active) {
                return false;
            }

            AudioManager.Instance.PlayMusic(_music);

            UIManager.Instance.Initialize();

            Core.Network.NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection);

            if(GameStateManager.Instance.GameManager.GameData.GamepadsArePlayers) {
                int count = Math.Min(Math.Max(InputManager.Instance.GamepadCount, 1), GameStateManager.Instance.GameManager.GameData.MaxLocalPlayers);

                Debug.Log($"Spawning a player for each controller ({count})...");
                for(short i=0; i<count; ++i) {
                    Core.Network.NetworkManager.Instance.AddLocalPlayer(i);
                }
            } else {
                Core.Network.NetworkManager.Instance.AddLocalPlayer(0);
            }

            return true;
        }
#endregion

#region Shutdown
        private IEnumerator<LoadStatus> ShutdownRoutine()
        {
            yield return new LoadStatus(0.0f, "Shutting down...");

            ShutdownClient();
            ShutdownServer();

            yield return new LoadStatus(0.25f, "Shutting down...");

            if(GameStateManager.HasInstance) {
                if(null != GameStateManager.Instance.GameManager) {
                    GameStateManager.Instance.GameManager.Shutdown();
                }

                GameStateManager.Instance.ShutdownNetwork();
            }

            yield return new LoadStatus(0.75f, "Shutting down...");

            PartyParrotManager.Instance.IsPaused = false;

            yield return new LoadStatus(1.0f, "Shutting down...");
        }

        protected virtual void ShutdownServer()
        {
            if(null != _serverSpectator) {
                Destroy(_serverSpectator.gameObject);
            }
            _serverSpectator = null;
        }

        protected virtual void ShutdownClient()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.FreeAllViewers();
            }

            if(UIManager.HasInstance) {
                UIManager.Instance.Shutdown();
            }

            if(AudioManager.HasInstance) {
                AudioManager.Instance.StopAllMusic();
            }
        }
#endregion

#region Event Handlers
        private void ServerDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: server disconnect");
        }

        private void ClientDisconnectEventHandler(object sender, EventArgs args)
        {
            Debug.LogError("TODO: client disconnect");
        }
#endregion
    }
}
