using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Collider))]
    public abstract class Actor3D : Actor
    {
#region Collider
        public Collider Collider { get; private set; }
#endregion

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Collider = GetComponent<Collider>();
        }
#endregion
    }
}
