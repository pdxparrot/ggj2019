using System;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
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

        [SerializeField]
        private EffectTrigger _respawnEffect;

        [SerializeField]
        private EffectTrigger _damageEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        [SerializeField]
        private EffectTrigger _gameOverEffect;

        [SerializeField]
        [ReadOnly]
        private bool _hasPollen;

        public bool HasPollen => _hasPollen;

        public bool CanGather => !IsDead && !HasPollen;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _respawnTimer = new Timer();

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

        public void AddPollen()
        {
            _hasPollen = true;
        }

        public void AddBeeToSwarm(NPCBee npcBee)
        {
            _swarm.Add(npcBee);
        }

        public bool Damage(int amount)
        {
            if(IsDead) {
                return false;
            }

            if(!_swarm.HasSwarm) {
                Kill();
                return true;
            }
            _swarm.Remove(amount);

            _damageEffect.Trigger();

            return true;
        }

        public void GameOver()
        {
            _gameOverEffect.Trigger();
        }

        private void Kill()
        {
            Behavior.Velocity = Vector3.zero;

            _isDead = true;

            _hasPollen = false;

            _swarm.RemoveAll();

            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(true);
            _deathEffect.Trigger();

            // despawn the actor (not the player)
            OnDeSpawn();

            GameManager.Instance.PlayerDeath();

            _respawnTimer.Start(PlayerManager.Instance.GamePlayerData.RespawnSeconds, Respawn);
        }

        public void DoGather()
        {
            if(IsDead) {
                return;
            }

            NPCBee npcBee = _interactables.GetBee();
            if(npcBee != null) {
                AddBeeToSwarm(npcBee);
            }
        }

        private void Respawn()
        {
            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(false);

            if(GameManager.Instance.IsGameOver) {
                return;
            }

            _isDead = false;

            PlayerManager.Instance.RespawnPlayer(this);

            _respawnEffect.Trigger();
        }
    }
}
