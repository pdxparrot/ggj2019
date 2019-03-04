#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
    public sealed class GameManager : GameManager<GameManager>
    {
#region Events
        public event EventHandler<EventArgs> GameStartEvent;
        public event EventHandler<EventArgs> GameEndEvent;
#endregion

        public GameData GameGameData => (GameData)GameData;

        // only valid on the client
        public GameViewer Viewer { get; private set; }

        [SerializeField]
        [ReadOnly]
        private bool _isGameOver;

        public override bool IsGameOver
        {
            get => _isGameOver;
            protected set => _isGameOver = value;
        }

#region Effects
        [SerializeField]
        private EffectTrigger _newWaveEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Stopwatch _gameTimer = new Stopwatch();

        [SerializeField]
        [ReadOnly]
        private int _score;

        public int Score => _score + (int)_gameTimer.StopwatchSeconds;

        public WaveSpawner WaveSpawner { get; private set; }

        public GameObject PollenContainer { get; private set; }

#region Unity Lifecycle
        private void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);

            Assert.IsTrue(GameData is GameData);
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

        public override void Initialize()
        {
            IsGameOver = false;

            InitObjectPools();

            InitWaveSpawner();

            PollenContainer = new GameObject("pollen");
            PollenContainer.transform.SetParent(transform);
        }

        public override void Shutdown()
        {
            _gameTimer.Stop();
            IsGameOver = false;

            Destroy(PollenContainer);
            PollenContainer = null;

            DestroyWaveSpawner();

            DestroyObjectPools();

            if(NetworkServer.active) {
                PlayerManager.Instance.DespawnPlayers();
            }
        }

#region Object Pools
        private void InitObjectPools()
        {
            PooledObject pooledObject = GameGameData.BeePrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePoolAsync("bees", pooledObject, GameGameData.BeePoolSize);

            pooledObject = GameGameData.PollenPrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePoolAsync("pollen", pooledObject, GameGameData.PollenPoolSize);

            pooledObject = GameGameData.FloatingTextPrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePoolAsync(UIManager.Instance.DefaultFloatingTextPoolName, pooledObject, GameGameData.FloatingTextPoolSize);
        }

        private void DestroyObjectPools()
        {
            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool(UIManager.Instance.DefaultFloatingTextPoolName);
                ObjectPoolManager.Instance.DestroyPool("pollen");
                ObjectPoolManager.Instance.DestroyPool("bees");
            }
        }
#endregion

#region Wave Spawner
        private void InitWaveSpawner()
        {
            // TODO: for some dumb reason, we're starting the game before all the players are "connected"
            // so we're just gonna have to assume we can count on this being correct
            int playerCount = Math.Min(InputManager.Instance.GamepadCount, GameGameData.MaxLocalPlayers);
            int waveSpawnerIndex = Math.Min(playerCount, GameGameData.WaveSpawnerPrefabs.Length) - 1;
            Assert.IsTrue(waveSpawnerIndex >= 0);

            Debug.Log($"Instantiating wave spawner configured for {waveSpawnerIndex + 1} players...");

            WaveSpawner = Instantiate(GameGameData.WaveSpawnerPrefabs[waveSpawnerIndex]);

            WaveSpawner.Initialize();
            WaveSpawner.WaveStartEvent += WaveStartEventHandler;
            WaveSpawner.WaveCompleteEvent += WaveCompleteEventHandler;
        }

        private void DestroyWaveSpawner()
        {
            if(null == WaveSpawner) {
                return;
            }

            WaveSpawner.WaveCompleteEvent -= WaveCompleteEventHandler;
            WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
            WaveSpawner.Shutdown();

            Destroy(WaveSpawner);
            WaveSpawner = null;
        }
#endregion

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);

            SpawnManager.Instance.Initialize();

            IsGameOver = false;
            GameStartEvent?.Invoke(this, EventArgs.Empty);

            WaveSpawner.StartSpawner();

            _score = 0;

            _gameTimer.Reset();
            _gameTimer.Start();
        }

        //[Server]
        public void EndGame()
        {
            Assert.IsTrue(NetworkServer.active);

            _gameTimer.Stop();

            WaveSpawner.StopSpawner();

            IsGameOver = true;
            GameEndEvent?.Invoke(this, EventArgs.Empty);

            foreach(Players.Player player in PlayerManager.Instance.Players) {
                player.GameOver();
            }

            // save high scores
            HighScoreManager.Instance.AddHighScore(PlayerManager.Instance.PlayerCount, Score);
        }

        //[Client]
        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>(gameObject);
            if(null != Viewer) {
                Viewer.Set2D();
                Viewer.Camera.orthographicSize = GameGameData.GameSize2D;
                Viewer.transform.position = GameGameData.ViewerPosition;
            }
        }

#region Score
        //[Server]
        public void PlayerDeath(IPlayer player)
        {
            _score -= GameGameData.DeathPenalty;
            if(_score < 0) {
                _score = 0;
            }

            Vector3 position = player.Behavior.Position;
            ShowScoreText(-GameGameData.DeathPenalty, GameGameData.NegativeFloatingTextColor, () => position);
        }

        //[Server]
        public void HiveDamage(Hive hive)
        {
            _score -= GameGameData.HiveDamagePenalty;
            if(_score < 0) {
                _score = 0;
            }

            Vector3 position = hive.transform.position;
            ShowScoreText(-GameGameData.HiveDamagePenalty, GameGameData.NegativeFloatingTextColor, () => position);
        }

        //[Server]
        public void PollenCollected(IPlayer player)
        {
            _score += GameGameData.PollenScore;

            Vector3 position = player.Behavior.Position;
            ShowScoreText(GameGameData.PollenScore, GameGameData.PositiveFloatingTextColor, () => {
                Players.Player gamePlayer = (Players.Player)player;
                return null == gamePlayer || gamePlayer.IsDead ? position : player.Behavior.Position;
            });
        }

        //[Server]
        public void BeetleKilled(IPlayer player)
        {
            _score += GameGameData.BeetleScore;

            Vector3 position = player.Behavior.Position;
            ShowScoreText(GameGameData.BeetleScore, GameGameData.PositiveFloatingTextColor, () => {
                Players.Player gamePlayer = (Players.Player)player;
                return null == gamePlayer || gamePlayer.IsDead ? position : player.Behavior.Position;
            });
        }

        //[Server]
        public void BeetleHarvest(Beetle beetle, int amount)
        {
            Vector3 position = beetle.transform.position;
            ShowScoreText(-amount, GameGameData.PollenFloatingTextColor, () => position);
        }

        //[Server]
        public void FlowerDestroyed(Flower flower)
        {
            Vector3 position = flower.transform.position;
            ShowScoreText(-GameGameData.FlowerDestroyedPenalty, GameGameData.NegativeFloatingTextColor, () => position);
        }

        //[Server]
        public void WaspKilled(IPlayer player)
        {
            _score += GameGameData.WaspScore;

            Vector3 position = player.Behavior.Position;
            ShowScoreText(GameGameData.WaspScore, GameGameData.PositiveFloatingTextColor, () => {
                Players.Player gamePlayer = (Players.Player)player;
                return null == gamePlayer || gamePlayer.IsDead ? position : player.Behavior.Position;
            });
        }

        private void ShowScoreText(int score, Color color, Func<Vector3> position)
        {
            if(score == 0) {
                return;
            }
            UIManager.Instance.QueueFloatingText(UIManager.Instance.DefaultFloatingTextPoolName, $"{score}", color, position);
        }
#endregion

#region Event Handlers
        // TODO: this should come from a server command
        //[Client]
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(args.WaveIndex == 0) {
                return;
            }

            _newWaveEffect.Trigger();
        }

        //[Server]
        private void WaveCompleteEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(args.IsFinalWave) {
                EndGame();
            }
        }
#endregion
    }
}
