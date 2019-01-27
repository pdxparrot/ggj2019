using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Core.Actors;

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Hive : PhysicsActor2D
    {
        [SerializeField] private int armorhealth;

        // [0 3]
        // [1 4]
        // [2 5]
        [SerializeField] private List<GameObject> _armor;
        [SerializeField] private float _topRow;
        [SerializeField] private float _bottomRow;

        private List<int> _health;

        public override float Height => Collider.bounds.size.y;
        public override float Radius => Collider.bounds.size.x;

        public override bool IsLocalActor => true;

        public override void OnSpawn() { }
        public override void OnReSpawn() { }

        [SerializeField] private int _maxBees = 5;
        [SerializeField] private float _beeSpawnCooldown = 10.0f;
        private readonly Timer _beeSpawnTimer = new Timer();

        [SerializeField]
        private NPCBee _beePrefab;

#region Unity Lifecycle
        protected override void Awake() {
            base.Awake();

            Pool.Add(this);

            _health = new List<int>();
            for(int i = 0; i < _armor.Count; ++i) {
                _health.Add(armorhealth);
            }
        }

        private void Update() {

            float dt = Time.deltaTime;
            _beeSpawnTimer.Update(dt);

            SpawnBee();
        }

        protected override void OnDestroy() {
            Pool.Remove(this);
            base.OnDestroy();
        }
#endregion

        public bool Collides(Bounds other, float distance = float.Epsilon) {
            Bounds bounds = Collider.bounds;
            bounds.Expand(distance);
            return bounds.Intersects(other);
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
            case 2: return 1;
            case 3: return 4;
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

            if(!armorLeft) {
                GameManager.Instance.EndGame();
            }

            return ret;
        }

        public void ShowDamage(int armoridx) {
            float f = (float)_health[armoridx] / (float)armorhealth;
            Color c = new Color(1, f, f);
            _armor[armoridx].GetComponent<SpriteRenderer>().color = c;
        }

        public bool TakeDamage(int armoridx, int d = 0) {
            if(_health[armoridx] > 0) {
                --_health[armoridx];

                if(_health[armoridx] == 0) {
                    _armor[armoridx].SetActive(false);
                    return true;
                }
                else {
                    ShowDamage(armoridx);
                }
            }
            else if(d < 3) {
                int n1 = neighbor1(armoridx);
                int n2 = neighbor2(armoridx);

                if(_health[n1] > 0)
                    return TakeDamage(n1);
                if(_health[n2] > 0)
                    return TakeDamage(n2);
                else
                    return true;
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

        public void UnloadPollen(int amount) {
            Debug.LogWarning($"TODO unload pollen");
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

        public static ProxPool<Hive> Pool = new ProxPool<Hive>();

        public static Hive Nearest(Vector3 pos, float dist = 1000000.0f) {
            return Pool.Nearest(pos, dist) as Hive;
        }
    }
}
