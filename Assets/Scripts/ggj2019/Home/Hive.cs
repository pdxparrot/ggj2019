#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class Hive : Actor2D
    {
#region Actor
        public override bool IsLocalActor => false;
#endregion

        [SerializeField]
        private HiveArmor[] _armor;

#region Debug
        [SerializeField]
        private bool _immune;

        public bool Immune => _immune;

        [SerializeField]
        private bool _logBeeSpawn;

        public bool LogBeeSpawn => _logBeeSpawn;

        private DebugMenuNode _debugMenuNode;
#endregion

        public HiveBehavior HiveBehavior => (HiveBehavior)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is HiveBehavior);

            Collider.isTrigger = true;

            InitDebugMenu();

            ActorManager.Instance.Register(this);
        }

        protected override void OnDestroy()
        {
            if(ActorManager.HasInstance) {
                ActorManager.Instance.Unregister(this);
            }

            DestroyDebugMenu();

            base.OnDestroy();
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is HiveBehaviorData);

            base.Initialize(id, data);

            foreach(HiveArmor armor in _armor) {
                armor.Initialize();
            }
        }

        public void InitializeClient()
        {
            Assert.IsTrue(NetworkClient.active);

            HiveBehavior.InitializeEffects();
        }

        public void CollectPollen(Player player)
        {
            HiveBehavior.OnCollectPollen(player);
        }

#region Events
        public void OnArmorDestroyed()
        {
            bool armorRemaining = false;
            foreach(HiveArmor armor in _armor) {
                if(armor.Health <= 0) {
                    continue;
                }

                armorRemaining = true;
                break;
            }

            HiveBehavior.OnArmorDestroyed(armorRemaining);
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ggj2019.Home.Hive");
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
