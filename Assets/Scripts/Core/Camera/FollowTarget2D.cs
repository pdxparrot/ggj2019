using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public class FollowTarget2D : MonoBehaviour
    {
        [SerializeField]
        private Collider2D _collider;

        public Collider2D Collider => _collider;

        [SerializeField]
        private Transform _targetTransform;

        public Transform TargetTransform
        {
            get => null == _targetTransform ? transform : _targetTransform;
            set => _targetTransform = value;
        }

        [SerializeField]
        [ReadOnly]
        private  Vector2 _lastLookAxes;

        public Vector2 LastLookAxes
        {
            get => _lastLookAxes;

            set => _lastLookAxes = value;
        }
    }
}
