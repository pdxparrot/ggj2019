using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    public abstract class FollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetTransform;

        public Transform TargetTransform
        {
            get => null == _targetTransform ? transform : _targetTransform;
            set => _targetTransform = value;
        }

        [SerializeField]
        [ReadOnly]
        private  Vector3 _lastLookAxes;

        public Vector3 LastLookAxes
        {
            get => _lastLookAxes;
            set => _lastLookAxes = value;
        }
    }
}
