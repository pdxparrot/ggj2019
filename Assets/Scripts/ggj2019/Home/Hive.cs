﻿using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

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

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider.isTrigger = true;

            _beeContainer = new GameObject("bees");
            _beeContainer.transform.SetParent(transform);

            GameManager.Instance.GameStartEvent += GameStartEventHandler;
            GameManager.Instance.GameEndEvent += GameEndEventHandler;

            InitDebugMenu();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _beeSpawnTimer.Update(dt);
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();

            if(GameManager.HasInstance) {
                GameManager.Instance.GameEndEvent -= GameEndEventHandler;
                GameManager.Instance.GameStartEvent -= GameStartEventHandler;
            }

            Destroy(_beeContainer);
        }
#endregion

        public void InitializeClient()
        {
            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

        public void ArmorDestroyed()
        {
            GameManager.Instance.HiveDamage();

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
            GameManager.Instance.PollenCollected();

            Bee bee = DoSpawnBee();
            if(null != bee && null != player) {
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
            if(activeBeeCount < GameManager.Instance.GameGameData.MinBees * PlayerManager.Instance.PlayerCount) {
                Bee bee = DoSpawnBee();
                //bee.SetRandomSkin();
                bee.SetDefaultSkin();
            } else if(_logBeeSpawn) {
                Debug.Log($"not spawning bees {activeBeeCount} of {GameManager.Instance.GameGameData.MinBees} active");
            }

            _beeSpawnTimer.Start(GameManager.Instance.GameGameData.BeeSpawnCooldown, SpawnBee);
        }

        private Bee DoSpawnBee()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");

            Bee bee = ObjectPoolManager.Instance.GetPooledObject<Bee>("bees");
            spawnPoint.Spawn(bee, Guid.NewGuid());
            bee.transform.SetParent(_beeContainer.transform);
            bee.Initialize(GameManager.Instance.GameGameData.BeeData);

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

            _beeSpawnTimer.Start(GameManager.Instance.GameGameData.BeeSpawnCooldown, SpawnBee);
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
