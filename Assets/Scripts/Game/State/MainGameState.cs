#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.UI;

using UnityEngine;
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

        public override void OnEnter()
        {
            base.OnEnter();

            Initialize();

            Core.Network.NetworkManager.Instance.ServerDisconnectEvent += ServerDisconnectEventHandler;
            Core.Network.NetworkManager.Instance.ClientDisconnectEvent += ClientDisconnectEventHandler;

            if(NetworkClient.active) {
                AudioManager.Instance.PlayMusic(_music);
                UIManager.Instance.Initialize();
            }
        }

        public override void OnUpdate(float dt)
        {
            if(GameStateManager.Instance.GameManager.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState, state => {
                    state.Initialize();
                });
            }
        }

        public override void OnExit()
        {
            UIManager.Instance.Shutdown();
            AudioManager.Instance.StopMusic();

            if(null != _serverSpectator) {
                Destroy(_serverSpectator);
            }
            _serverSpectator = null;

            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerDisconnectEvent -= ServerDisconnectEventHandler;
                Core.Network.NetworkManager.Instance.ClientDisconnectEvent -= ClientDisconnectEventHandler;
            }

            if(UIManager.HasInstance) {
                UIManager.Instance.Shutdown();
            }

            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.ShutdownNetwork();
            }

            base.OnExit();
        }

        private void Initialize()
        {
            PartyParrotManager.Instance.IsPaused = false;

            DebugMenuManager.Instance.ResetFrameStats();

            InitializeServer();
            InitializeClient();
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

            Core.Network.NetworkManager.Instance.LocalClientReady(GameStateManager.Instance.NetworkClient?.connection, 0);

            return true;
        }

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