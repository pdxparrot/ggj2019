using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    // TODO: rename FollowCameraTarget3D
    public class FollowTarget3D : FollowTarget
    {
        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;
    }
}
