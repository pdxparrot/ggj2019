#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
    public sealed class GameManager : Game.GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        public override bool IsGameOver { get; protected set; }

        [SerializeField]
        [ReadOnly]
        private Stopwatch _gameTimer;

        [SerializeField]
        [ReadOnly]
        private int _currentWave;

        public int CurrentWave => _currentWave;

        public int Score => (int)_gameTimer.StopwatchSeconds;

#region Unity Lifecycle
        private void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _gameTimer.Update(dt);
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterGameManager();
            }

            base.OnDestroy();
        }
#endregion

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);

            SpawnManager.Instance.Initialize();

            IsGameOver = false;

            _gameTimer.Reset();
            _gameTimer.Start();

            _currentWave = 0;

            NPCSpawner.Instance.Initialize();
            NPCSpawner.Instance.WaveStartEvent += OnWaveStarted;

            Hive.Instance.Initialize();
        }

        //[Server]
        public void EndGame()
        {
            IsGameOver = true;

            _gameTimer.Stop();
        }

        //[Client]
        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null != Viewer) {
                Viewer.Set2D();
                Viewer.Camera.orthographicSize = GameGameData.GameSize2D;
                Viewer.transform.position = GameGameData.ViewerPosition;
            }
        }

        private void OnWaveStarted()
        {
            _currentWave++;
            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowWaveText(_currentWave);
        }
    }
}
