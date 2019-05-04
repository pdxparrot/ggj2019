using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class HiveBehavior : ActorBehavior2D
    {
        [Space(10)]

        [Header("Hive")]

#region Effects
        [SerializeField]
        private EffectTrigger _endGameExplosion;

        [SerializeField]
        private EffectTrigger _endGameExplosionBig;

        [SerializeField]
        private EffectTrigger _damageEffect;
#endregion

#region Bee Spawning
        private ITimer _beeSpawnTimer;
#endregion

        private Hive Hive => (Hive)Owner;

        public HiveBehaviorData HiveBehaviorData => (HiveBehaviorData)BehaviorData;

#region Unity Lifecycle
        protected override void Awake()
        {
            GameManager.Instance.GameStartEvent += GameStartEventHandler;
            GameManager.Instance.GameEndEvent += GameEndEventHandler;

            _beeSpawnTimer = TimeManager.Instance.AddTimer();
            _beeSpawnTimer.TimesUpEvent += BeeSpawnTimerTimesUpEventHandler;

            base.Awake();
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_beeSpawnTimer);
            }
            _beeSpawnTimer = null;

            if(GameManager.HasInstance) {
                GameManager.Instance.GameEndEvent -= GameEndEventHandler;
                GameManager.Instance.GameStartEvent -= GameStartEventHandler;
            }

            base.OnDestroy();
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is HiveBehaviorData);

            base.Initialize(behaviorData);
        }

        public void InitializeEffects()
        {
            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

#region Bee Actions
        private void SpawnBee()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            int activeBeeCount = ActorManager.Instance.ActorCount<Bee>();
            if(activeBeeCount < HiveBehaviorData.MinBees * PlayerManager.Instance.Players.Count) {
                DoSpawnBee();
            } else if(Hive.LogBeeSpawn) {
                Debug.Log($"not spawning bees {activeBeeCount} of {HiveBehaviorData.MinBees} active");
            }

            _beeSpawnTimer.Start(HiveBehaviorData.BeeSpawnCooldown);
        }

        private Bee DoSpawnBee()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");

            Bee bee = ObjectPoolManager.Instance.GetPooledObject<Bee>("bees", GameManager.Instance.BeeContainer.transform);
            spawnPoint.Spawn(bee, Guid.NewGuid(), GameManager.Instance.GameGameData.BeeBehaviorData);

            bee.SetDefaultSkin();
            //bee.SetRandomSkin();

            return bee;
        }
#endregion

#region Events
        public void OnArmorDestroyed(bool armorRemaining)
        {
            GameManager.Instance.HiveDamage(Hive);

            _damageEffect.Trigger();

            if(armorRemaining) {
                return;
            }

            // no armor remaining, the game is over
            _endGameExplosion.Trigger(() => {
                _endGameExplosionBig.Trigger();
            });

            GameManager.Instance.EndGame();
        }

        public void OnCollectPollen(Player player)
        {
            GameManager.Instance.PollenCollected(player);

            Bee bee = DoSpawnBee();
            if(null == bee) {
                return;
            }

            if(null != player && !player.IsDead) {
                player.AddBeeToSwarm(bee);
            }
        }
#endregion

#region Event Handlers
        private void GameStartEventHandler(object sender, EventArgs args)
        {
            DoSpawnBee();

            _beeSpawnTimer.Start(HiveBehaviorData.BeeSpawnCooldown);
        }

        private void GameEndEventHandler(object sender, EventArgs args)
        {
            _beeSpawnTimer.Stop();
        }

        private void BeeSpawnTimerTimesUpEventHandler(object sender, EventArgs args)
        {
            SpawnBee();
        }
#endregion
    }
}
