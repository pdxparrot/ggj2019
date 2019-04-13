#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class Hive : Actor2D
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        private HiveArmor[] _armor;

#region Effects
        [SerializeField]
        private EffectTrigger _endGameExplosion;

        [SerializeField]
        private EffectTrigger _endGameExplosionBig;

        [SerializeField]
        private EffectTrigger _damageEffect;
#endregion

#region Bee Spawning
        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _beeSpawnTimer = new Timer();

        private GameObject _beeContainer;
#endregion

#region Debug
        [SerializeField]
        private bool _immune;

        public bool Immune => _immune;

        [SerializeField]
        private bool _logBeeSpawn;

        private DebugMenuNode _debugMenuNode;
#endregion

        public HiveBehavior HiveBehavior => (HiveBehavior)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is HiveBehavior);

            Collider.isTrigger = true;

            _beeContainer = new GameObject("bees");

            GameManager.Instance.GameStartEvent += GameStartEventHandler;
            GameManager.Instance.GameEndEvent += GameEndEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            if(GameManager.HasInstance) {
                GameManager.Instance.GameEndEvent -= GameEndEventHandler;
                GameManager.Instance.GameStartEvent -= GameStartEventHandler;
            }

            Destroy(_beeContainer);

            base.OnDestroy();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _beeSpawnTimer.Update(dt);
        }
#endregion

        public void Initialize(HiveBehaviorData behaviorData)
        {
            Behavior.Initialize(behaviorData);
        }

        public void InitializeClient()
        {
            Assert.IsTrue(NetworkClient.active);

            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

        public void ArmorDestroyed()
        {
            GameManager.Instance.HiveDamage(this);

            _damageEffect.Trigger();

            // check to see if we have any armor left
            foreach(HiveArmor armor in _armor) {
                if(armor.Health > 0) {
                    return;
                }
            }

            // no armor left, the game is over
            _endGameExplosion.Trigger(() => {
                _endGameExplosionBig.Trigger();
            });
            GameManager.Instance.EndGame();
        }

        public void CollectPollen(Players.Player player)
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

#region Bee Spawning
        private void SpawnBee()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            int activeBeeCount = ActorManager.Instance.ActorCount<Bee>();
            if(activeBeeCount < HiveBehavior.HiveBehaviorData.MinBees * PlayerManager.Instance.Players.Count) {
                DoSpawnBee();
            } else if(_logBeeSpawn) {
                Debug.Log($"not spawning bees {activeBeeCount} of {HiveBehavior.HiveBehaviorData.MinBees} active");
            }

            _beeSpawnTimer.Start(HiveBehavior.HiveBehaviorData.BeeSpawnCooldown, SpawnBee);
        }

        private Bee DoSpawnBee()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");

            Bee bee = ObjectPoolManager.Instance.GetPooledObject<Bee>("bees", _beeContainer.transform);
            spawnPoint.Spawn(bee, Guid.NewGuid(), GameManager.Instance.GameGameData.BeeBehaviorData);

            bee.SetDefaultSkin();
            //bee.SetRandomSkin();

            return bee;
        }
#endregion

#region Event Handlers
        private void GameStartEventHandler(object sender, EventArgs args)
        {
            foreach(HiveArmor armor in _armor) {
                armor.Initialize();
            }

            DoSpawnBee();

            _beeSpawnTimer.Start(HiveBehavior.HiveBehaviorData.BeeSpawnCooldown, SpawnBee);
        }

        private void GameEndEventHandler(object sender, EventArgs args)
        {
            _beeSpawnTimer.Stop();
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ggj2019.Hive");
            _debugMenuNode.RenderContentsAction = () => {
                _immune = GUILayout.Toggle(_immune, "Immune");
                _logBeeSpawn = GUILayout.Toggle(_logBeeSpawn, "Log Bee Spawn");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
