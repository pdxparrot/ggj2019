#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
// TODO: move pollen spawning into a manager
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

        private WaveSpawner _waveSpawner;

        public WaveSpawner WaveSpawner => _waveSpawner;

        private int _score;

        public int Score => _score + (int)_gameTimer.StopwatchSeconds;

#region Unity Lifecycle
        private void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);

            _waveSpawner = Instantiate(GameGameData.WaveSpawnerPrefab);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _gameTimer.Update(dt);
            _pollenTimer.Update(dt);
        }

        protected override void OnDestroy()
        {
            Destroy(_waveSpawner);

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

            _score = 0;

            Hive.Instance.Initialize();

            _pollenTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_pollenDelayMin, _pollenDelayMax), SpawnPollen);

            _waveSpawner.Initialize();
            _waveSpawner.WaveStartEvent += WaveStartEventHandler;
            _waveSpawner.WaveCompleteEvent += WaveCompleteEventHandler;
            _waveSpawner.StartSpawner();
        }

        //[Server]
        public void EndGame()
        {
            _waveSpawner.StopSpawner();
            _waveSpawner.WaveCompleteEvent += WaveCompleteEventHandler;
            _waveSpawner.WaveStartEvent += WaveStartEventHandler;
            _waveSpawner.Shutdown();

            Hive.Instance.Shutdown();

            IsGameOver = true;

            _pollenTimer.Stop();
            _gameTimer.Stop();
        }

        //[Server]
        private void SpawnPollen()
        {
            // give it to a random flower
            for(int i=0; i<10; ++i) {
                NPCFlower flower = NPCFlower.Flowers.GetRandomEntry();
                if(null != flower && flower.IsReady && !flower.IsDead && flower.CanSpawnPollen) {
                    flower.SpawnPollen();
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

#region Event Handlers
        // TODO: this should come from a server command
        //[Client]
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(null != UIManager.Instance.PlayerUI) {
                ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowWaveText(args.WaveIndex);
            }
        }

        // TODO: this should come from a server command
        //[Client]
        private void WaveCompleteEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(args.IsFinalWave) {
                EndGame();
            }
        }
#endregion
    }
}
