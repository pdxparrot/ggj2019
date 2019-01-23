using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public class FollowTarget3D : FollowTarget
    {
        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;
    }
}
