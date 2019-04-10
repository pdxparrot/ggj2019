#pragma warning disable 0618    // disable obsolete warning for now

using System.Collections.Generic;

using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Players.BehaviorComponents;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(BoundsPlayerBehaviorComponent2D))]
    public sealed class PlayerBehavior : Game.Players.PlayerBehavior2D
    {
        public Data.PlayerBehaviorData GamePlayerBehaviorData => (Data.PlayerBehaviorData)PlayerBehaviorData;

        public Player GamePlayer => (Player)Player;

        [Space(10)]

        [Header("Game Player")]

#region State
        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead => _isDead;

        // start true to force the animation the first time
        // TODO: is this actually necessary?
        [SerializeField]
        [ReadOnly]
        private bool _isFlying = true;
#endregion

        [Space(10)]

#region Effects
        [Header("Player Effects")]

        [SerializeField]
        private EffectTrigger _damageEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        [SerializeField]
        private EffectTrigger _gameOverEffect;
#endregion

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ List<Pollen> _pollen = new List<Pollen>();

        public int PollenCount => _pollen.Count;

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

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is PlayerBehaviorData);

            base.Initialize(behaviorData);

            BoundsPlayerBehaviorComponent2D boundsBehavior = GetBehaviorComponent<BoundsPlayerBehaviorComponent2D>();
            boundsBehavior.Initialize(GameManager.Instance.GameGameData);
        }

        public void InitializeEffects()
        {
            Assert.IsTrue(NetworkClient.active);

            Assert.IsNotNull(GamePlayer.GamePlayerDriver.GamepadListener);
            Assert.IsNotNull(GamePlayer.Viewer);

            RumbleEffectTriggerComponent rumbleEffect = _respawnEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _damageEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _deathEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            rumbleEffect = _gameOverEffect.GetEffectTriggerComponent<RumbleEffectTriggerComponent>();
            rumbleEffect.GamepadListener = GamePlayer.GamePlayerDriver.GamepadListener;

            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GamePlayer.Viewer;

            viewerShakeEffect = _deathEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GamePlayer.Viewer;
        }

        protected override void AnimationUpdate(float dt)
        {
            base.AnimationUpdate(dt);

            if(IsMoving) {
                SetFlyingAnimation();
            } else  {
                SetIdleAnimation();
            }
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(!_isFlying) {
                return;
            }

            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(GamePlayerBehaviorData.IdleAnimationName, true);
            }
            _isFlying = false;
        }

        private void SetFlyingAnimation()
        {
            if(_isFlying) {
                return;
            }

            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(GamePlayerBehaviorData.FlyingAnimationName, true);
            }
            _isFlying = true;
        }
#endregion

#region Events
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            Velocity = Vector3.zero;

            _isDead = false;

            if(null != AnimationHelper) {
                AnimationHelper.SetFacing(Vector3.zero - transform.position);
            }
            SetIdleAnimation();
        }

        public override void OnReSpawn(SpawnPoint spawnpoint)
        {
            base.OnReSpawn(spawnpoint);

            _isDead = false;

            _immunityTimer.Start(GamePlayerBehaviorData.SpawnImmunitySeconds);

            if(null != AnimationHelper) {
                AnimationHelper.SetFacing(Vector3.zero - transform.position);
            }
            SetIdleAnimation();
        }

        public override void OnDeSpawn()
        {
            _pollen.Clear();

            Velocity = Vector3.zero;

            _respawnTimer.Start(GamePlayerBehaviorData.RespawnSeconds, ReSpawn);

            base.OnDeSpawn();
        }

        public override void TriggerEnter(GameObject triggerObject)
        {
            base.TriggerEnter(triggerObject);

            UnloadPollen(triggerObject.GetComponent<Hive>());
        }

        public override void TriggerStay(GameObject triggerObject)
        {
            base.TriggerStay(triggerObject);

            UnloadPollen(triggerObject.GetComponent<Hive>());
        }

        public override void TriggerExit(GameObject triggerObject)
        {
            base.TriggerExit(triggerObject);

            UnloadPollen(triggerObject.GetComponent<Hive>());
        }

        public void OnAddPollen(Pollen pollen)
        {
            _pollen.Add(pollen);
        }

        public void OnDamage()
        {
            if(_isDead) {
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
            if(_isDead) {
                return;
            }

            var interactables = GamePlayer.Interactables.GetInteractables<Bee>();
            Bee bee = (Bee)interactables.GetRandomEntry();
            if(null != bee) {
                GamePlayer.AddBeeToSwarm(bee);
            }
        }
#endregion

#region Actions
        private void ReSpawn()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            PlayerManager.Instance.RespawnPlayer(GamePlayer);
        }

        private void Kill()
        {
            _isDead = true;

            GameManager.Instance.PlayerDeath(GamePlayer);

            _deathEffect.Trigger();

            // despawn the actor (not the player)
            GamePlayer.OnDeSpawn();
        }

        private void UnloadPollen(Hive hive)
        {
            if(null == hive || _pollen.Count < 1) {
                return;
            }

            foreach(Pollen pollen in _pollen) {
                pollen.Unload(hive);
            }
            _pollen.Clear();
        }
#endregion
    }
}
