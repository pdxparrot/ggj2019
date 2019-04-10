using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    // TODO: rename FollowCameraTarget2D
    public class FollowTarget2D : FollowTarget
    {
        [SerializeField]
        private Collider2D _collider;

        public Collider2D Collider => _collider;
    }
}
