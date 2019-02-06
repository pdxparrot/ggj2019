using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Actor2D : Actor
    {
#region Collider
        public Collider2D Collider { get; private set; }
#endregion

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Collider = GetComponent<Collider2D>();
        }
#endregion
    }
}
