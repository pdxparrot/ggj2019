using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class Player : Player2D
    {
        public PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        private PlayerDriver GamePlayerDriver => (PlayerDriver)PlayerDriver;

        [SerializeField]
        private Transform _pollenTarget;

        public Transform PollenTarget => _pollenTarget;

        [SerializeField]
        private Interactables _interactables;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead => _isDead;

#region Effects
        [SerializeField]
        private EffectTrigger _spawnEffect;

        [SerializeField]
        private EffectTrigger _respawnEffect;

        [SerializeField]
        private EffectTrigger _damageEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        [SerializeField]
        private EffectTrigger _gameOverEffect;
#endregion

        private readonly List<Pollen> _pollen = new List<Pollen>();

        public bool HasPollen => _pollen.Count > 0;

        public bool CanGather => !IsDead && (PlayerManager.Instance.GamePlayerData.MaxPollen < 0 || _pollen.Count < PlayerManager.Instance.GamePlayerData.MaxPollen);

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _respawnTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _immunityTimer = new Timer();

        private bool IsImmune => PlayerManager.Instance.PlayersImmune || _immunityTimer.IsRunning;

        public int SkinIndex => NetworkPlayer.playerControllerId;

        private Swarm _swarm;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _swarm = GetComponent<Swarm>();
            _skinHelper = GetComponent<SpineSkinHelper>();

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _respawnTimer.Update(dt);
            _immunityTimer.Update(dt);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            UnloadPollen(other.gameObject.GetComponent<Hive>());
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            UnloadPollen(other.gameObject.GetComponent<Hive>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            UnloadPollen(other.gameObject.GetComponent<Hive>());
        }
#endregion

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;
            _skinHelper.SetSkin(SkinIndex);

            RumbleEffectTriggerComponent rumbleEffect = _respawnEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayerDriver.GamepadListener;

            rumbleEffect = _damageEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayerDriver.GamepadListener;

            rumbleEffect = _deathEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayerDriver.GamepadListener;

            rumbleEffect = _gameOverEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayerDriver.GamepadListener;

            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;

            viewerShakeEffect = _deathEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;

            return true;
        }

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _spawnEffect.Trigger();

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            _isDead = false;

            _respawnEffect.Trigger();
            _immunityTimer.Start(PlayerManager.Instance.GamePlayerData.SpawnImmunitySeconds);

            return true;
        }

        public override void OnDeSpawn()
        {
            Behavior.Velocity = Vector3.zero;

            _pollen.Clear();

            _swarm.RemoveAll();

            _respawnTimer.Start(PlayerManager.Instance.GamePlayerData.RespawnSeconds, Respawn);

            base.OnDeSpawn();
        }
#endregion

        public void AddPollen(Pollen pollen)
        {
            _pollen.Add(pollen);
        }

        public void AddBeeToSwarm(Bee bee)
        {
            _swarm.Add(bee);
            _interactables.RemoveInteractable(bee);

            bee.SetPlayerSkin(this);
        }

        public bool Damage()
        {
            if(IsDead) {
                return false;
            }

            if(!IsImmune) {
                if(!_swarm.HasSwarm) {
                    Kill();
                    return true;
                }
                _swarm.Remove(1);
            }

            _damageEffect.Trigger();

            return true;
        }

        public void GameOver()
        {
            _gameOverEffect.Trigger();
        }

        private void Kill()
        {
            _isDead = true;

            GameManager.Instance.PlayerDeath(this);

            _deathEffect.Trigger();

            // despawn the actor (not the player)
            OnDeSpawn();
        }

        public void DoGather()
        {
            if(IsDead) {
                return;
            }

            var interactables = _interactables.GetInteractables<Bee>();
            Bee bee = (Bee)interactables.GetRandomEntry();
            if(null != bee) {
                AddBeeToSwarm(bee);
            }
        }

        private void UnloadPollen(Hive hive)
        {
            if(null == hive || !HasPollen) {
                return;
            }

            foreach(Pollen pollen in _pollen) {
                pollen.Unload(hive);
            }
            _pollen.Clear();
        }

        private void Respawn()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            PlayerManager.Instance.RespawnPlayer(this);
        }
    }
}
