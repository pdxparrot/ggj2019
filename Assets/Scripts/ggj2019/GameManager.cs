#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.State;
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
            InitObjectPools();

            InitWaveSpawner();

            PollenContainer = new GameObject("pollen");
            PollenContainer.transform.SetParent(transform);
        }

        public override void Shutdown()
        {
            Destroy(PollenContainer);
            PollenContainer = null;

            DestroyWaveSpawner();

            DestroyObjectPools();
        }

#region Object Pools
        private void InitObjectPools()
        {
            PooledObject pooledObject = GameGameData.BeePrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePool("bees", pooledObject, GameGameData.BeePoolSize);

            pooledObject = GameGameData.PollenPrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePool("pollen", pooledObject, GameGameData.PollenPoolSize);

            pooledObject = GameGameData.FloatingTextPrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePool("floating_text", pooledObject, GameGameData.FloatingTextPoolSize);
        }

        private void DestroyObjectPools()
        {
            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool("floating_text");
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
            WaveSpawner.WaveCompleteEvent -= WaveCompleteEventHandler;
            WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
            WaveSpawner.Shutdown();

            Destroy(WaveSpawner);
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

            // save high scores and then kick everyone
            HighScoreManager.Instance.AddHighScore(PlayerManager.Instance.PlayerCount, Score);
            PlayerManager.Instance.DespawnPlayers();
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

            ShowScoreText(player.Behavior.Position, GameGameData.NegativeFloatingTextColor, GameGameData.DeathPenalty);
        }

        //[Server]
        public void HiveArmorDamage(HiveArmor armor)
        {
            ShowScoreText(armor.transform.position, GameGameData.NegativeFloatingTextColor, "Damage");
        }

        //[Server]
        public void HiveDamage(Hive hive)
        {
            _score -= GameGameData.HiveDamagePenalty;
            if(_score < 0) {
                _score = 0;
            }

            ShowScoreText(hive.transform.position, GameGameData.NegativeFloatingTextColor, GameGameData.HiveDamagePenalty);
        }

        //[Server]
        public void PollenCollected(IPlayer player)
        {
            _score += GameGameData.PollenScore;

            ShowScoreText(player.Behavior.Position, GameGameData.PositiveFloatingTextColor, GameGameData.PollenScore);
        }

        //[Server]
        public void BeetleKilled(IPlayer player)
        {
            _score += GameGameData.BeetleScore;

            ShowScoreText(player.Behavior.Position, GameGameData.PositiveFloatingTextColor, GameGameData.BeetleScore);
        }

        //[Server]
        public void BeetleHarvest(Beetle beetle, int amount)
        {
            ShowScoreText(beetle.transform.position, GameGameData.PollenFloatingTextColor, amount);
        }

        //[Server]
        public void WaspKilled(IPlayer player)
        {
            _score += GameGameData.WaspScore;

            ShowScoreText(player.Behavior.Position, Color.green, GameGameData.WaspScore);
        }

        // TODO: this should use a queue so that the numbers don't clump
        // TODO: we also need to contain the text in something until it recycles
        private void ShowScoreText(Vector3 position, Color color, int score)
        {
            FloatingText floatingText = ObjectPoolManager.Instance.GetPooledObject<FloatingText>("floating_text");
            floatingText.Text.text = $"{score}";
            floatingText.Text.color = color;
            floatingText.Show(position);
        }

        private void ShowScoreText(Vector3 position, Color color, string text)
        {
            FloatingText floatingText = ObjectPoolManager.Instance.GetPooledObject<FloatingText>("floating_text");
            floatingText.Text.text = text;
            floatingText.Text.color = color;
            floatingText.Show(position);
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
