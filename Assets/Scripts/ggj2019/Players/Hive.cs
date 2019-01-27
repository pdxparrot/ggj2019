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
            Pool.Add(this);
        }

        protected void OnDestroy() {
            Pool.Remove(this);
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

        public static ProxPool<Hive> Pool = new ProxPool<Hive>();

        public static Hive Nearest(Vector3 pos, float dist = 1000000.0f) {
            return Pool.Nearest(pos, dist) as Hive;
        }
    }
}
