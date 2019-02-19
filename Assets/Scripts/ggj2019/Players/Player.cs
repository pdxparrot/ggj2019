using System;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.NPCs.Control;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerBehavior => (PlayerController)PlayerBehavior;

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

        [SerializeField]
        [ReadOnly]
        private bool _hasPollen;

        public bool HasPollen => _hasPollen;

        public bool CanGather => !IsDead && !HasPollen;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _respawnTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _immunityTimer = new Timer();

        private bool IsImmune => PlayerManager.Instance.PlayersImmune || _immunityTimer.IsRunning;

        private Swarm _swarm;

        private SpineSkinSwapper _skinSwapper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _swarm = GetComponent<Swarm>();
            _skinSwapper = GetComponent<SpineSkinSwapper>();

            Assert.IsTrue(PlayerBehavior is PlayerController);
        }

        private void Update()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            if(HasPollen && Hive.Instance.Collides(this)) {
                Hive.Instance.UnloadPollen(this, 1);
                _hasPollen = false;
            }

            float dt = Time.deltaTime;

            _respawnTimer.Update(dt);
            _immunityTimer.Update(dt);
        }
#endregion

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;
            _skinSwapper.SetSkin(NetworkPlayer.playerControllerId);

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

            _hasPollen = false;

            _swarm.RemoveAll();

            _respawnTimer.Start(PlayerManager.Instance.GamePlayerData.RespawnSeconds, Respawn);

            base.OnDeSpawn();
        }
#endregion

        public void AddPollen()
        {
            _hasPollen = true;
        }

        public void AddBeeToSwarm(NPCBee bee)
        {
            _swarm.Add(bee);
            _interactables.RemoveInteractable(bee);
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

            GameManager.Instance.PlayerDeath();

            _deathEffect.Trigger();

            // despawn the actor (not the player)
            OnDeSpawn();
        }

        public void DoGather()
        {
            if(IsDead) {
                return;
            }

            var interactables = _interactables.GetInteractables<NPCBee>();
            NPCBee bee = (NPCBee)interactables.GetRandomEntry();
            if(null != bee) {
                AddBeeToSwarm(bee);
            }
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
