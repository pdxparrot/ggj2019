using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.ggj2019.NPCs;

using DG.Tweening;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Hive : PhysicsActor2D
    {
        // TODO: do this better
        public static Hive Instance { get; private set; }

        [SerializeField] private int armorhealth;

        // [0 3]
        // [1 4]
        // [2 5]
        [SerializeField] private List<GameObject> _armor;
        [SerializeField] private float _topRow;
        [SerializeField] private float _bottomRow;

        [SerializeField] private EffectTrigger _endGameExplosion;
        [SerializeField] private EffectTrigger _endGameExplosionBig;
        [SerializeField] private EffectTrigger _damageEffect;
        [SerializeField] private GameObject _hiveBackground;

        private readonly List<int> _health = new List<int>();

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

        public override bool IsLocalActor => true;

        [SerializeField] private int _maxBees = 5;
        [SerializeField] private float _beeSpawnCooldown = 10.0f;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _beeSpawnTimer = new Timer();

        [SerializeField]
        private NPCBee _beePrefab;

        private GameObject _beeContainer;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Instance = this;

            for(int i=0; i<_armor.Count; ++i) {
                _health.Add(armorhealth);
            }

            _beeContainer = new GameObject("bees");
            _beeContainer.transform.SetParent(transform);

            PooledObject pooledObject = _beePrefab.GetComponent<PooledObject>();
            ObjectPoolManager.Instance.InitializePool("bees", pooledObject, _maxBees * 4);

            GameManager.Instance.GameStartEvent += GameStartEventHandler;
            GameManager.Instance.GameEndEvent += GameEndEventHandler;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _beeSpawnTimer.Update(dt);
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.GameEndEvent -= GameEndEventHandler;
                GameManager.Instance.GameStartEvent -= GameStartEventHandler;
            }

            if(ObjectPoolManager.HasInstance) {
                ObjectPoolManager.Instance.DestroyPool("bees");
            }
            Destroy(_beeContainer);

            Instance = null;
        }
#endregion

        private int neighbor1(int pc) {
            switch(pc) {
            default:
            case 0: return 3;
            case 1: return 4;
            case 2: return 5;
            case 3: return 0;
            case 4: return 1;
            case 5: return 2;
        }}
        private int neighbor2(int pc) {
            switch(pc) {
            default:
            case 0: return 1;
            case 1: return 2;
            case 2: return -1;
            case 3: return 4;
            case 4: return 5;
            case 5: return -1;
        }}
        private int neighbor3(int pc) {
            switch(pc) {
            default:
            case 0: return -1;
            case 1: return 0;
            case 2: return 1;
            case 3: return -1;
            case 4: return 3;
            case 5: return 4;
        }}

        public bool TakeDamage(Vector3 pos) {
            int armoridx = ((pos.x > 0.0f) ? 3 : 0) +
                           ((pos.y > _topRow) ? 0 :
                            (pos.y > _bottomRow) ? 1 : 2);
            bool ret = TakeDamage(armoridx);

            _damageEffect.Trigger();

            bool armorLeft = false;
            for(int i=0; i<_health.Count; ++i) {
                if(_health[i] > 0) {
                    armorLeft = true;
                }
            }

            if(!armorLeft)
            {
                EndAnimation();
                GameManager.Instance.EndGame();
            }

            return ret;
        }

        public void ShowDamage(int armoridx) {
            float f = (float)_health[armoridx] / (float)armorhealth;
            Color c = new Color(1, f, f);
            _armor[armoridx].GetComponent<SpriteRenderer>().color = c;
            _armor[armoridx].GetComponent<EffectTrigger>().Trigger();
            _armor[armoridx].transform.DOShakePosition(.3f,  0.3f, 20, 130f);
        }

        public void DestroyArmor(int armoridx)
        {
            _armor[armoridx].transform.DOShakePosition(
                .1f,
                0.3f,
                20,
                130f)
                .OnComplete(() => _armor[armoridx].GetComponent<SpriteRenderer>().enabled = false);
            _armor[armoridx].GetComponent<EffectTrigger>().Trigger(
                () => _armor[armoridx].SetActive(false)
            );
        }

        private void EndAnimation()
        {
            _endGameExplosion.Trigger(EndExplosion);
            transform.DOShakePosition(5f, 0.3f, 20, 130f);
        }

        private void EndExplosion()
        {
            _hiveBackground.gameObject.SetActive(false);
            _endGameExplosionBig.Trigger();
        }

        private bool TakeDamage(int armoridx, bool recurse = true) {
            if(_health[armoridx] > 0) {
                --_health[armoridx];

                if(_health[armoridx] == 0) {
                    DestroyArmor(armoridx);
                    return true;
                }
                else {
                    ShowDamage(armoridx);
                }
            }
            else if(recurse) {
                int n1 = neighbor1(armoridx);
                int n2 = neighbor2(armoridx);
                int n3 = neighbor3(armoridx);

                if(_health[n1] > 0
                || (n2 != -1 && _health[n2] > 0)
                || (n3 != -1 && _health[n3] > 0)) {
                    bool result  = TakeDamage(n1, false);
                         if(n2 != -1)
                             result |= TakeDamage(n2, false);
                         if(n3 != -1)
                             result |= TakeDamage(n3, false);
                    return result;
                }
                else {
                    bool result  = TakeDamage(0, false);
                         result |= TakeDamage(1, false);
                         result |= TakeDamage(2, false);
                         result |= TakeDamage(3, false);
                         result |= TakeDamage(4, false);
                         result |= TakeDamage(5, false);
                    return result;
                }
            }

            return false;
        }

        public int UnloadPollen(Player player, int amount)
        {
            NPCBee bee = DoSpawnBee();
            if(null != bee && null != player) {
                player.AddBeeToSwarm(bee);
            }
            return amount;
        }

        private void SpawnBee()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            if(NPCBee.Bees.Count < _maxBees) {
                DoSpawnBee();
            }

            _beeSpawnTimer.Start(_beeSpawnCooldown, SpawnBee);
        }

        [CanBeNull]
        private NPCBee DoSpawnBee()
        {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");
            if(spawnPoint == null) {
                return null;
            }

            NPCBee bee = ObjectPoolManager.Instance.GetPooledObject<NPCBee>("bees");
            spawnPoint.Spawn(bee, Guid.NewGuid());
            bee.transform.SetParent(_beeContainer.transform);

            return bee;
        }

#region Event Handlers
        private void GameStartEventHandler(object sender, EventArgs args)
        {
            DoSpawnBee();

            _beeSpawnTimer.Start(_beeSpawnCooldown, SpawnBee);
        }

        private void GameEndEventHandler(object sender, EventArgs args)
        {
            _beeSpawnTimer.Stop();
        }
#endregion
    }
}
