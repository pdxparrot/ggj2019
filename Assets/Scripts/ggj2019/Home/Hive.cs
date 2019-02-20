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

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class Hive : Actor2D
    {
        // TODO: do this better
        public static Hive Instance { get; private set; }

        public override bool IsLocalActor => false;

#region Armor
        // [0 3]
        // [1 4]
        // [2 5]
        [SerializeField]
        private HiveArmor[] _armor;

        [SerializeField]
        private float _topRow;

        [SerializeField]
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

        private DebugMenuNode _debugMenuNode;
#endregion

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
#endregion

        public void InitializeClient()
        {
            ViewerShakeEffectTriggerComponent viewerShakeEffect = _damageEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
            viewerShakeEffect.Viewer = GameManager.Instance.Viewer;
        }

#region Damage (clean this up)
// TODO: the HiveArmor can have its neighbors linked....
        private int neighbor1(int pc)
        {
            switch(pc)
            {
            default:
            case 0: return 3;
            case 1: return 4;
            case 2: return 5;
            case 3: return 0;
            case 4: return 1;
            case 5: return 2;
            }
        }

        private int neighbor2(int pc)
        {
            switch(pc)
            {
            default:
            case 0: return 1;
            case 1: return 2;
            case 2: return -1;
            case 3: return 4;
            case 4: return 5;
            case 5: return -1;
            }
        }

        private int neighbor3(int pc)
        {
            switch(pc)
            {
            default:
            case 0: return -1;
            case 1: return 0;
            case 2: return 1;
            case 3: return -1;
            case 4: return 3;
            case 5: return 4;
            }
        }

        public bool Damage(Vector3 pos)
        {
            bool armorDestroyed = false;

            if(!_immune) {
                // figure out which armor piece was hit
                int armoridx = ((pos.x > 0.0f) ? 3 : 0) +
                               ((pos.y > _topRow) ? 0 :
                                (pos.y > _bottomRow) ? 1 : 2);

                // damage the armor
                int count = 0;
                armorDestroyed = Damage(armoridx, true, ref count);
                //UnityEngine.Assertions.Assert.IsTrue(count <= 1, $"Damaged {count} armor pieces!");
                if(armorDestroyed) {
                    GameManager.Instance.HiveDamage();
                    _damageEffect.Trigger();
                }

                // check to see if we have any armor left
                bool armorLeft = false;
                foreach(HiveArmor armor in _armor) {
                    if(armor.Health > 0) {
                        armorLeft = true;
                    }
                }

                // if no armor left, the game is over
                if(!armorLeft) {
                    _endGameExplosion.Trigger(() => {
                        _endGameExplosionBig.Trigger();
                    });
                    GameManager.Instance.EndGame();
                }
            }

            return armorDestroyed;
        }

        private bool Damage(int armoridx, bool recurse, ref int count)
        {
            HiveArmor armor = _armor[armoridx];
            if(armor.Health > 0) {
                count++;
                if(armor.Damage(1)) {
                    return true;
                }
            } else if(recurse) {
                int n1 = neighbor1(armoridx);
                int n2 = neighbor2(armoridx);
                int n3 = neighbor3(armoridx);

                if(_armor[n1].Health > 0 || (n2 != -1 && _armor[n2].Health > 0) || (n3 != -1 && _armor[n3].Health > 0)) {
                    bool result  = Damage(n1, false, ref count);
                    if(n2 != -1)
                        result |= Damage(n2, false, ref count);
                    if(n3 != -1)
                        result |= Damage(n3, false, ref count);
                    return result;
                } else {
                    bool result  = Damage(0, false, ref count);
                         result |= Damage(1, false, ref count);
                         result |= Damage(2, false, ref count);
                         result |= Damage(3, false, ref count);
                         result |= Damage(4, false, ref count);
                         result |= Damage(5, false, ref count);
                    return result;
                }
            }

            return false;
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

            if(ActorManager.Instance.ActorCount<Bee>() < GameManager.Instance.GameGameData.MinBees) {
                DoSpawnBee();
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
