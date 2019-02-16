#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
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
#region Events
        public event EventHandler<EventArgs> GameStartEvent;
        public event EventHandler<EventArgs> GameEndEvent;
#endregion

        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        public override bool IsGameOver { get; protected set; }

#region Effects
        [SerializeField]
        private EffectTrigger _newWaveEffect;

        [SerializeField]
        private EffectTrigger _gameOverEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Stopwatch _gameTimer = new Stopwatch();

        [SerializeField]
        [ReadOnly]
        private int _score;

        public int Score => _score + (int)_gameTimer.StopwatchSeconds;

        public WaveSpawner WaveSpawner { get; private set; }

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

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);

            IsGameOver = false;

            SpawnManager.Instance.Initialize();

            _score = 0;

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

            GameStartEvent?.Invoke(this, EventArgs.Empty);

            WaveSpawner.StartSpawner();

            _gameTimer.Reset();
            _gameTimer.Start();
        }

        //[Server]
        public void EndGame()
        {
            _gameTimer.Stop();

            WaveSpawner.StopSpawner();

            GameEndEvent?.Invoke(this, EventArgs.Empty);

            WaveSpawner.WaveCompleteEvent -= WaveCompleteEventHandler;
            WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
            WaveSpawner.Shutdown();
            Destroy(WaveSpawner);

            foreach(Players.Player player in PlayerManager.Instance.Players) {
                player.GameOver();
            }

            // save high scores and then kick everyone
            HighScoreManager.Instance.AddHighScore(PlayerManager.Instance.PlayerCount, Score);
            PlayerManager.Instance.DespawnPlayers();

            IsGameOver = true;
            _gameOverEffect.Trigger();
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
        public void HiveDamage()
        {
            _score -= GameGameData.HiveDamagePenalty;
            if(_score < 0) {
                _score = 0;
            }
        }

        //[Server]
        public void PollenCollected()
        {
            _score += GameGameData.PollenScore;
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
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>(gameObject);
            if(null != Viewer) {
                Viewer.Set2D();
                Viewer.Camera.orthographicSize = GameGameData.GameSize2D;
                Viewer.transform.position = GameGameData.ViewerPosition;
            }

            ViewerShakeEffectTriggerComponent viewerShakeEffect = _gameOverEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = Viewer;
        }

#region Event Handlers
        // TODO: this should come from a server command
        //[Client]
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(args.WaveIndex == 0) {
                return;
            }

            _newWaveEffect.Trigger();

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
