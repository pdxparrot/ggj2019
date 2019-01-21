#pragma warning disable 0618    // disable obsolete warning for now

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Camera;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.State
{
    public sealed class GameStateManager : SingletonBehavior<GameStateManager>
    {
#region Game State Prefabs
        [Header("Game States")]

        [SerializeField]
        private MainMenuState _mainMenuStatePrefab;

        [SerializeField]
        private NetworkConnectState _networkConnectStatePrefab;

        [SerializeField]
        private MainGameState _gameStatePrefab;

        [SerializeField]
        private SceneTester _sceneTesterStatePrefab;

        [SerializeField]
        [ReadOnly]
        private IGameState _currentGameState;

        [CanBeNull]
        public IGameState CurrentState => _currentGameState;

        private readonly Stack<IGameState> _stateStack = new Stack<IGameState>();
#endregion

        [Space(10)]

#region Server Spectator
        [Header("Server Spectator")]

        [SerializeField]
        private ServerSpectator _serverSpectatorPrefab;

        public  ServerSpectator ServerSpectatorPrefab => _serverSpectatorPrefab;

        [SerializeField]
        private ServerSpectatorViewer _serverSpectatorViewer;

        public ServerSpectatorViewer ServerSpectatorViewer => _serverSpectatorViewer;
#endregion

#region Network
        [CanBeNull]
        public NetworkClient NetworkClient { get; set; }
#endregion

#region Unity Lifecycle
        private void Awake()
        {
// TODO: allocate and disable *all* game states

            InitDebugMenu();
        }

        private void OnDestroy()
        {
            ExitCurrentState(null);

// TODO: destroy all game states

            base.OnDestroy();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _currentGameState?.OnUpdate(dt);
        }
#endregion

        public void ShutdownNetwork()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.Stop();
            }
            NetworkClient = null;
        }

#region State Management
        public void TransitionToInitialState(Action<GameState> initializeState=null, Action onStateLoaded=null)
        {
            Debug.Log("Transition to initial state");
            TransitionState(_mainMenuStatePrefab, initializeState, onStateLoaded);
        }

        public void TransitionState<TV>(TV gameStatePrefab, Action<TV> initializeState=null, Action onStateLoaded=null) where TV: GameState
        {
            PartyParrotManager.Instance.LoadingManager.ShowLoadingScreen(true);

            ExitCurrentState(() => {
                // TODO: this should enable the state from the set rather than allocating
                TV gameState = Instantiate(gameStatePrefab, transform);
                initializeState?.Invoke(gameState);

                PartyParrotManager.Instance.LoadingManager.UpdateLoadingScreen(0.5f, "Loading scene...");
                gameState.LoadScene(() => {
                    _currentGameState = gameState;
                    _currentGameState.OnEnter();

                    PartyParrotManager.Instance.LoadingManager.ShowLoadingScreen(false);

                    onStateLoaded?.Invoke();
                });
            });
        }

        private void ExitCurrentState(Action callback)
        {
            if(null == _currentGameState) {
                callback?.Invoke();
                return;
            }

            while(_stateStack.Count > 0 && !(_currentGameState is GameState)) {
                PopSubState();
            }

            GameState gameState = (GameState)_currentGameState;
            _currentGameState = null;

            gameState.UnloadScene(() => {
                if(null != gameState) {
                    gameState.OnExit();

                    // TODO: disable the state, don't destroy it
                    Destroy(gameState.gameObject);
                }

                callback?.Invoke();
            });
        }

        public void PushSubState<TV>(TV gameStatePrefab, Action<TV> initializeState=null) where TV: SubGameState
        {
            _currentGameState?.OnPause();

            // enqueue the current state if we have one
            if(null != _currentGameState) {
                _stateStack.Push(_currentGameState);
            }

            // new state is now the current state
            // TODO: this should enable the state from the set rather than allocating
            TV gameState = Instantiate(gameStatePrefab, transform);
            initializeState?.Invoke(gameState);

            _currentGameState = gameState;
            _currentGameState.OnEnter();
        }

        public void PopSubState()
        {
            SubGameState previousState = (SubGameState)_currentGameState;
            _currentGameState = null;

            if(null != previousState) {
                previousState.OnExit();
                Destroy(previousState.gameObject);
            }

            _currentGameState = _stateStack.Count > 0 ? _stateStack.Pop() : null;
            _currentGameState?.OnResume();
        }
#endregion

#region Start Game
        public void StartSinglePlayer()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnectState.ConnectType.SinglePlayer, _gameStatePrefab);
            });
        }

        public void StartHost()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnectState.ConnectType.Server, _gameStatePrefab);
            });
        }

        public void StartJoin()
        {
            PushSubState(_networkConnectStatePrefab, state => {
                state.Initialize(NetworkConnectState.ConnectType.Client, _gameStatePrefab);
            });
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.GameStateManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Game State: {CurrentState?.Name}");

                if(GUIUtils.LayoutButton("Reset")) {
                    TransitionToInitialState();
                }

                if(null != NetworkClient) {
                    GUILayout.BeginVertical("Client Stats", GUI.skin.box);
                        GUILayout.Label($"Ping: {NetworkClient.GetRTT()}ms");

                        NetworkClient.GetStatsIn(out int numMsgs, out int numBytes);
                        GUILayout.Label($"Messages received: {numMsgs}");
                        GUILayout.Label($"Bytes received: {numBytes}");

                        NetworkClient.GetStatsOut(out numMsgs, out int numBufferedMsgs, out numBytes, out int lastBufferedPerSecond);
                        GUILayout.Label($"Messages sent: {numMsgs}");
                        GUILayout.Label($"Messages buffered: {numBufferedMsgs}");
                        GUILayout.Label($"Bytes sent: {numBytes}");
                        GUILayout.Label($"Messages buffered per second: {lastBufferedPerSecond}");
                    GUILayout.EndVertical();
                }

                if(NetworkServer.active) {
                    GUILayout.BeginVertical("Server Stats", GUI.skin.box);
                    NetworkServer.GetStatsIn(out int numMsgs, out int numBytes);
                        GUILayout.Label($"Messages received: {numMsgs}");
                        GUILayout.Label($"Bytes received: {numBytes}");

                        NetworkServer.GetStatsOut(out numMsgs, out int numBufferedMsgs, out numBytes, out int lastBufferedPerSecond);
                        GUILayout.Label($"Messages sent: {numMsgs}");
                        GUILayout.Label($"Messages buffered: {numBufferedMsgs}");
                        GUILayout.Label($"Bytes sent: {numBytes}");
                        GUILayout.Label($"Messages buffered per second: {lastBufferedPerSecond}");
                    GUILayout.EndVertical();
                }

                foreach(string sceneName in _sceneTesterStatePrefab.TestScenes) {
                    if(GUIUtils.LayoutButton($"Load Test Scene {sceneName}")) {
                        TransitionToInitialState(null, () => {
                            PushSubState(_networkConnectStatePrefab, connectState => {
                                connectState.Initialize(NetworkConnectState.ConnectType.SinglePlayer, _sceneTesterStatePrefab, state => {
                                    SceneTester sceneTester = (SceneTester)state;
                                    sceneTester.SetScene(sceneName);
                                });
                            });
                        });
                        break;
                    }
                }
            };
        }
    }
}
