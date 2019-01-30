#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core;
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
    public sealed class GameManager : GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        public override bool IsGameOver { get; protected set; }

        [SerializeField] private float _pollenDelayMin = 3;
        [SerializeField] private float _pollenDelayMax = 6;

        private readonly Timer _pollenTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private Stopwatch _gameTimer;

        [SerializeField]
        [ReadOnly]
        private int _currentWave;

        public int CurrentWave => _currentWave;

        private int _score;

        public int Score => _score + (int)_gameTimer.StopwatchSeconds;

#region Unity Lifecycle
        private void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _gameTimer.Update(dt);
            _pollenTimer.Update(dt);
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
            _score = 0;

            NPCSpawner.Instance.Initialize();
            NPCSpawner.Instance.WaveStartEvent += OnWaveStarted;

            Hive.Instance.Initialize();

            _pollenTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_pollenDelayMin, _pollenDelayMax), SpawnPollen);
        }

        //[Server]
        public void EndGame()
        {
            NPCSpawner.Instance.WaveStartEvent -= OnWaveStarted;

            IsGameOver = true;

            _gameTimer.Stop();
        }

        //[Server]
        private void SpawnPollen()
        {
            // -- give it to a random flower
            for(int i=0; i<10; ++i) {
                var f = NPCFlower.Pool.Random();
                if(null != f && f.IsReady && !f.IsDead && f.CanSpawnPollen) {
                    f.SpawnPollen();
                    break;
                }
            }
            _pollenTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_pollenDelayMin, _pollenDelayMax), SpawnPollen);
        }

        //[Server]
        public void PlayerDeath()
        {
            _score -= GameGameData.DeathPenalty;
            if(_score < 0) {
                _score = 0;
            }
        }

        //[Server]
        public void BeetleKilled()
        {
            _score += GameGameData.BeetleScore;
        }

        //[Server]
        public void WaspKilled()
        {
            _score += GameGameData.WaspScore;
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
