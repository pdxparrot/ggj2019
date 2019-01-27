using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Core.Actors;

using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Hive : Actor
    {
        [SerializeField] private int armorhealth;
        [SerializeField] private float height;
        [SerializeField] private float radius;
        [SerializeField] private float yscale;

        // [0 3]
        // [1 4]
        // [2 5]
        [SerializeField] private List<GameObject> _armor;
        [SerializeField] private float _topRow;
        [SerializeField] private float _bottomRow;

        private List<int> _health;

        public override float Height { get { return height; } }
        public override float Radius { get { return radius; } }

        public override bool IsLocalActor { get { return true; } }

        public override void OnSpawn() { }
        public override void OnReSpawn() { }

#region Unity Lifecycle
        protected override void Awake() {
            base.Awake();

            Pool.Add(this);

            _health = new List<int>();
            for(int i = 0; i < _armor.Count; ++i) {
                _health.Add(armorhealth);
            }
        }

        protected override void OnDestroy() {
            Pool.Remove(this);
            base.OnDestroy();
        }
#endregion

        public bool Collides(Vector3 pos) {
            Vector3 offset = pos - transform.position;
            offset.y /= yscale; // since we're an ellipse

            return (Vector3.Magnitude(offset) < radius);
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

        public static ProxPool<Hive> Pool = new ProxPool<Hive>();

        public static Hive Nearest(Vector3 pos, float dist = 1000000.0f) {
            return Pool.Nearest(pos, dist) as Hive;
        }
    }
}
