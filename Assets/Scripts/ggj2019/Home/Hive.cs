using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class Hive : Actor2D
    {
        // TODO: do this better
        public static Hive Instance { get; private set; }

        public override bool IsLocalActor => false;

#region Armor
        [SerializeField]
        private HiveArmor[] _armor;

        [SerializeField]
        [Tooltip("Local Y position of the bottom of the top row (red)")]
        private float _topRow;

        [SerializeField]
        [Tooltip("Local Y position of the top of the bottom row (green)")]
        private float _bottomRow;
#endregion

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

        [SerializeField]
        private bool _logBeeSpawn;

        private DebugMenuNode _debugMenuNode;
#endregion

        private float TopRow => transform.position.y + _topRow;

        private float BottomRow => transform.position.y + _bottomRow;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Instance = this;

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

            Instance = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-3.0f, TopRow), new Vector3(2.5f, TopRow));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(-3.0f, BottomRow), new Vector3(2.5f, BottomRow));
        }
#endregion

        public void InitializeClient()
        {
            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

#region Damage
        private int ArmorIndex(Vector3 pos)
        {
            return (pos.x > 0.0f ? 3 : 0) +         // left / right side
                    (pos.y >= TopRow ? 0 :         // top row or above
                        pos.y <= BottomRow ? 2 :   // bottom row or below
                            1);                     // middle
        }

        public bool Damage(Vector3 pos)
        {
            if(_immune) {
                return false;
            }

            // figure out which armor piece was hit
            int armoridx = ArmorIndex(pos);

            // damage the armor
            bool armorDestroyed = _armor[armoridx].Damage();
            if(armorDestroyed) {
                GameManager.Instance.HiveDamage();
                _damageEffect.Trigger();
            }

            // check to see if we have any armor left
            bool armorLeft = false;
            foreach(HiveArmor armor in _armor) {
                if(armor.Health > 0) {
                    armorLeft = true;
                    break;
                }
            }

            // if no armor left, the game is over
            if(!armorLeft) {
                _endGameExplosion.Trigger(() => {
                    _endGameExplosionBig.Trigger();
                });
                GameManager.Instance.EndGame();
            }

            return armorDestroyed;
        }
#endregion

        public int UnloadPollen(Players.Player player, int amount)
        {
            GameManager.Instance.PollenCollected();

            Bee bee = DoSpawnBee();
            if(null != bee && null != player) {
                player.AddBeeToSwarm(bee);
            }

            return amount;
        }

#region Bee Spawning
        private void SpawnBee()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            int activeBeeCount = ActorManager.Instance.ActorCount<Bee>();
            if(activeBeeCount < GameManager.Instance.GameGameData.MinBees) {
                DoSpawnBee();
            } else if(_logBeeSpawn) {
                Debug.Log($"not spawning bees {activeBeeCount} of {GameManager.Instance.GameGameData.MinBees} active");
            }

            _beeSpawnTimer.Start(GameManager.Instance.GameGameData.BeeSpawnCooldown, SpawnBee);
        }

        [CanBeNull]
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
