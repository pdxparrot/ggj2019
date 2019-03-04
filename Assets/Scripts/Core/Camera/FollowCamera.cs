using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    //[RequireComponent(typeof(Viewer))]
    public abstract class FollowCamera : MonoBehaviour
    {
#region Zoom
        [Header("Zoom")]

        [SerializeField]
        [Tooltip("Enable zooming in and out relative to the follow target")]
        private bool _enableZoom;

        protected bool EnableZoom => _enableZoom;

        [SerializeField]
        [Range(0, 10)]
        private float _minZoomDistance = 5.0f;

        protected float MinZoomDistance => _minZoomDistance;

        [SerializeField]
        [Range(0, 100)]
        private float _maxZoomDistance = 100.0f;

        protected float MaxZoomDistance => _maxZoomDistance;

        [SerializeField]
        [Range(0, 500)]
        private float _zoomSpeed = 500.0f;

        protected float ZoomSpeed => _zoomSpeed;
#endregion

        [Space(10)]

#region Smooth
        [SerializeField]
        [Tooltip("Smooth the camera movement as it follows the target")]
        private bool _smooth;

        protected bool Smooth => _smooth;

        [SerializeField]
        [Range(0, 0.5f)]
        private float _smoothTime = 0.05f;

        protected float SmoothTime => _smoothTime;

        [SerializeField]
        [ReadOnly]
        private Vector3 _velocity;

        protected Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }
#endregion

        [Space(10)]

#region Target
        [Header("Target")]

        [SerializeField]
        [Tooltip("The target to follow")]
        [CanBeNull]
        private FollowTarget _target;

        [CanBeNull]
        public FollowTarget Target => _target;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastTargetPosition;

        protected Vector3 LastTargetPosition
        {
            get => _lastTargetPosition;
            set => _lastTargetPosition = value;
        }
#endregion

#region Unity Lifecycle
        protected virtual void Update()
        {
            float dt = Time.deltaTime;

            HandleInput(dt);
        }

        protected virtual void LateUpdate()
        {
            if(_smooth) {
                return;
            }

            float dt = Time.deltaTime;

            FollowTarget(dt);
        }

        protected virtual void FixedUpdate()
        {
            if(!_smooth) {
                return;
            }

            float dt = Time.fixedDeltaTime;

            FollowTarget(dt);
        }
#endregion

        public virtual void SetTarget(FollowTarget target)
        {
            _target = target;
        }

        protected abstract void HandleInput(float dt);

        protected abstract void FollowTarget(float dt);
    }
}
