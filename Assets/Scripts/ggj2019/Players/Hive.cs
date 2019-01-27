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
        [SerializeField] private float height;
        [SerializeField] private float radius;
        [SerializeField] private float yscale;

        // [0 3]
        // [1 4]
        // [2 5]
        [SerializeField] private List<GameObject> _armor;
        [SerializeField] private float _topRow;
        [SerializeField] private float _bottomRow;

        public override float Height { get { return height; } }
        public override float Radius { get { return radius; } }

        public override bool IsLocalActor { get { return true; } }

        public override void OnSpawn() { }
        public override void OnReSpawn() { }

#region Unity Lifecycle
        protected override void Awake() {
            base.Awake();

            if(_hives == null)
                _hives = new List<Hive>();
            _hives.Add(this);
        }

        protected void OnDestroy() {
            for(int i = 0; i < _hives.Count; ++i) {
                if(_hives[i] == this) {
                    _hives.RemoveAt(i);
                    return;
                }
            }
        }

        protected void Update() {
        }
#endregion

        public bool Collides(Vector3 pos) {
            Vector3 offset = pos - transform.position;
            offset.y /= yscale; // since we're an ellipse

            return (Vector3.Magnitude(offset) < radius);
        }

        public void TakeDamage(Vector3 pos) {
            int armoridx = ((pos.x > 0.0f) ? 3 : 0) +
                           ((pos.y > _topRow) ? 0 :
                            (pos.y > _bottomRow) ? 1 : 2);

            _armor[armoridx].SetActive(false);
        }

        public void Repair() {
// TODO
        }

        private static List<Hive> _hives;

        public static Hive Nearest(Vector3 pos) {
            int best = -1;
            float bestT = 0.0f;
            if(_hives == null)
                return null;

            for(int i = 0; i < _hives.Count; ++i) {
                float dist = (_hives[i].transform.position - pos).magnitude;
                if(best == -1 || dist < bestT) {
                    bestT = dist;
                    best = i;
                }
            }

            return (best >= 0) ? _hives[best] : null;
        }
    }
}
