using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Core.Actors;

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using DG.Tweening;
using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.World;
using Spine.Unity;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Hive : PhysicsActor2D
    {
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
        [SerializeField] private GameObject _hiveBackground;

        private List<int> _health;

        public override float Height => Collider.bounds.size.y / 2.0f;
        public override float Radius => Collider.bounds.size.x / 2.0f;

        public override bool IsLocalActor => true;

        [SerializeField] private int _maxBees = 5;
        [SerializeField] private float _beeSpawnCooldown = 10.0f;
        private readonly Timer _beeSpawnTimer = new Timer();

        [SerializeField]
        private NPCBee _beePrefab;

#region Unity Lifecycle
        protected override void Awake() {
            base.Awake();

            Instance = this;

            //Pool.Add(this);

            _health = new List<int>();
            for(int i = 0; i < _armor.Count; ++i) {
                _health.Add(armorhealth);
            }
        }

        private void Update() {
            if(!_initialized || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;
            _beeSpawnTimer.Update(dt);

            SpawnBee();
        }

        protected override void OnDestroy() {
            Instance = null;

            //Pool.Remove(this);

            base.OnDestroy();
        }
#endregion

        private bool _initialized;

        public void Initialize() {
            _initialized = true;
        }

        [SerializeField]
        private SkeletonAnimation _animation;

        private void SetAnimation(string animationName, bool loop)
        {
            _animation.AnimationState.SetAnimation(0, animationName, loop);
        }

        public bool Collides(Actor actor, float distance = 0.0f) {
            Vector3 offset = actor.transform.position - transform.position;
            float r = actor.Radius + Radius;
            return Vector3.Magnitude(offset) < r + distance;
        }

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

        public bool TakeDamage(int armoridx, bool recurse = true) {
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

        public void Repair() {
            for(int i = 0; i < _armor.Count; ++i) {
                if(_health[i] > 0 && _health[i] < armorhealth) {
                    ShowDamage(i);
                    ++_health[i];
                }
            }
        }

        public int UnloadPollen(Player player, int amount) {
            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");
            if(spawnPoint != null) {
                var bee = spawnPoint.SpawnPrefab(_beePrefab) as NPCBee;

                if(null != player) {
                    player.AddBeeToSwarm(bee);
                }
            }
            return amount;
        }

        private void SpawnBee() {
            if(_beeSpawnTimer.IsRunning || NPCBee.Pool.Count >= _maxBees) {
                return;
            }

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint("bee");
            if(spawnPoint != null) {
                spawnPoint.SpawnPrefab(_beePrefab);
            }

            _beeSpawnTimer.Start(_beeSpawnCooldown);
        }

        //public static ProxPool<Hive> Pool = new ProxPool<Hive>();

        public static Hive Nearest(Vector3 pos, float dist = 1000000.0f) {
            return Instance;//Pool.Nearest(pos, dist) as Hive;
        }
    }
}
