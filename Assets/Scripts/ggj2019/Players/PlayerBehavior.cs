using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerBehavior : Game.Players.PlayerBehavior2D
    {
        public Data.PlayerBehaviorData GamePlayerBehaviorData => (Data.PlayerBehaviorData)PlayerBehaviorData;

        public Player GamePlayer => (Player)Player;

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

        // start true to force the animation the first time
        // TODO: is this actually necessary?
        [SerializeField]
        [ReadOnly]
        private bool _isFlying = true;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _respawnTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _immunityTimer = new Timer();

        public bool IsImmune => PlayerManager.Instance.PlayersImmune ||  _immunityTimer.IsRunning;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerBehaviorData is Data.PlayerBehaviorData);
        }

        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            _respawnTimer.Update(dt);
            _immunityTimer.Update(dt);
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();

            InitializeEffects();

            SpineAnimation.SetFacing(Vector3.zero - transform.position);
            SetIdleAnimation();
        }

        public void InitializeEffects()
        {
            RumbleEffectTriggerComponent rumbleEffect = _respawnEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _damageEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _deathEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _gameOverEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;

            viewerShakeEffect = _deathEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

        public override void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            base.DefaultAnimationMove(axes, dt);

            if(IsMoving) {
                SetFlyingAnimation();
            } else  {
                SetIdleAnimation();
            }
        }

#region Actions
        public void OnSpawn()
        {
            GamePlayer.IsDead = false;

            _spawnEffect.Trigger();
        }

        public void OnReSpawn()
        {
            GamePlayer.IsDead = false;

            _respawnEffect.Trigger();

            _immunityTimer.Start(PlayerManager.Instance.GamePlayerData.SpawnImmunitySeconds);
        }

        public void OnDeSpawn()
        {
            Velocity = Vector3.zero;

            _respawnTimer.Start(PlayerManager.Instance.GamePlayerData.RespawnSeconds, ReSpawn);
        }

        public void OnCollide(GameObject collideObject)
        {
            UnloadPollen(collideObject.GetComponent<Hive>());
        }

        public void OnDamage()
        {
            if(GamePlayer.IsDead) {
                return;
            }

            if(!IsImmune) {
                if(!GamePlayer.Swarm.HasSwarm) {
                    Kill();
                    return;
                }
                GamePlayer.Swarm.Remove(1);
            }

            _damageEffect.Trigger();
        }

        public void OnGameOver()
        {
            _gameOverEffect.Trigger();
        }

        public void OnGather()
        {
            if(GamePlayer.IsDead) {
                return;
            }

            var interactables = GamePlayer.Interactables.GetInteractables<Bee>();
            Bee bee = (Bee)interactables.GetRandomEntry();
            if(null != bee) {
                GamePlayer.AddBeeToSwarm(bee);
            }
        }
#endregion

        private void ReSpawn()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            PlayerManager.Instance.RespawnPlayer(GamePlayer);
        }

        private void Kill()
        {
            GamePlayer.IsDead = true;

            GameManager.Instance.PlayerDeath(GamePlayer);

            _deathEffect.Trigger();

            // despawn the actor (not the player)
            GamePlayer.OnDeSpawn();
        }

        private void UnloadPollen(Hive hive)
        {
            if(null == hive || GamePlayer.Pollen.Count < 1) {
                return;
            }

            foreach(Pollen pollen in GamePlayer.Pollen) {
                pollen.Unload(hive);
            }
            GamePlayer.Pollen.Clear();
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(!_isFlying) {
                return;
            }

            SpineAnimation.SetAnimation(GamePlayerBehaviorData.IdleAnimationName, true);
            _isFlying = false;
        }

        private void SetFlyingAnimation()
        {
            if(_isFlying) {
                return;
            }

            SpineAnimation.SetAnimation(GamePlayerBehaviorData.FlyingAnimationName, true);
            _isFlying = true;
        }
#endregion
    }
}
