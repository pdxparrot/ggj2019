using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public class FollowTarget2D : FollowTarget
    {
        [SerializeField]
        private Collider2D _collider;

        public Collider2D Collider => _collider;
    }
}
