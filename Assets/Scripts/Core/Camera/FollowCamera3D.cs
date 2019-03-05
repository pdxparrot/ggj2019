using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Math;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Camera
{
    public class FollowCamera3D : FollowCamera
    {
        [Space(10)]

#region Orbit
        [Header("Orbit")]

        [SerializeField]
        [Tooltip("Enable looking around the follow target")]
        private bool _enableOrbit = true;

        [SerializeField]
        [Range(0, 500)]
        private float _orbitSpeedX = 100.0f;

        [SerializeField]
        [Range(0, 500)]
        private float _orbitSpeedY = 100.0f;

        [SerializeField]
        [Tooltip("Return to the default orbit rotation when released")]
        private bool _returnToDefault;

        [SerializeField]
        private Vector2 _defaultOrbitRotation = new Vector2(0.0f, 30.0f);

        [SerializeField]
        [Range(0, 1)]
        private float _defaultOrbitReturnTime = 0.5f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _orbitReturnVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector2 _orbitRotation;

        // TODO: use this
        /*[SerializeField]
        [Range(0, 50)]
        [Tooltip("The minimum distance the camera should be from the follow target")]
        private float _orbitMinRadius = 15.0f;*/

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("The maximum distance the camera should be from the follow target")]
        private float _orbitMaxRadius = 15.0f;
#endregion

#region Orbit Constraints
        [SerializeField]
        [Range(-360, 0)]
        private float _orbitXMin = -90.0f;

        [SerializeField]
        [Range(0, 360)]
        private float _orbitXMax = 90.0f;

        [SerializeField]
        [Range(-360, 0)]
        private float _orbitYMin = -90.0f;

        [SerializeField]
        [Range(0, 360)]
        private float _orbitYMax = 90.0f;
#endregion

        [Space(10)]

#region Look
        [Header("Look")]

        [SerializeField]
        [Tooltip("Enable rotating the camera around its local axes")]
        private bool _enableLook;

        [SerializeField]
        [Range(0, 100)]
        private float _lookSpeedX = 100.0f;

        [SerializeField]
        [Range(0, 100)]
        private float _lookSpeedY = 100.0f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _lookRotation;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _isLooking;

        public bool IsLooking => _isLooking;

        [CanBeNull]
        public FollowTarget3D Target3D => (FollowTarget3D)Target;

        public override void SetTarget(FollowTarget target)
        {
            Assert.IsTrue(Target is FollowTarget3D);

            base.SetTarget(target);
            _orbitRotation = _defaultOrbitRotation;
        }

        protected override void HandleInput(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera3D.HandleInput");
            try {
                Vector3 lastLookAxes = Target.LastLookAxes;
                _isLooking = lastLookAxes.sqrMagnitude > ActorBehavior.AxesDeadZone;

                Orbit(lastLookAxes, dt);
                Zoom(lastLookAxes, dt);
                Look(lastLookAxes, dt);
            } finally {
                Profiler.EndSample();
            }
        }

        private void Orbit(Vector3 axes, float dt)
        {
            if(!_enableOrbit) {
                return;
            }

            // TODO: this is fighting too hard at max rotation
            // (or maybe the max rotation clamping is killing it)
            if(_returnToDefault) {
                _orbitRotation = Vector2.SmoothDamp(_orbitRotation, _defaultOrbitRotation, ref _orbitReturnVelocity, _defaultOrbitReturnTime, Mathf.Infinity, dt);
            }

            _orbitRotation.x = Mathf.Clamp(MathUtil.WrapAngle(_orbitRotation.x + axes.x * _orbitSpeedX * dt), _orbitXMin, _orbitXMax);
            _orbitRotation.y = Mathf.Clamp(MathUtil.WrapAngle(_orbitRotation.y - axes.y * _orbitSpeedY * dt), _orbitYMin, _orbitYMax);
        }

        private void Zoom(Vector3 axes, float dt)
        {
            if(!EnableZoom) {
                return;
            }

            float zoomAmount = axes.z * ZoomSpeed * dt;

            float minDistance = MinZoomDistance, maxDistance = MaxZoomDistance;
            if(null != Target3D) {
                // avoid zooming into the target
                Vector3 closestBoundsPoint = Target3D.Collider.ClosestPointOnBounds(transform.position);
                float distanceToPoint = (closestBoundsPoint - Target3D.TargetTransform.position).magnitude;

                minDistance += distanceToPoint;
                maxDistance += distanceToPoint;

                _orbitMaxRadius = Mathf.Clamp(_orbitMaxRadius + zoomAmount, minDistance, maxDistance);
            } else {
                _orbitMaxRadius += zoomAmount;
            }
        }

        private void Look(Vector3 axes, float dt)
        {
            if(!_enableLook) {
                return;
            }

            _lookRotation.x = MathUtil.WrapAngle(_lookRotation.x + axes.x * _lookSpeedX * dt);
            _lookRotation.y = MathUtil.WrapAngle(_lookRotation.y - axes.y * _lookSpeedY * dt);
        }

        protected override void FollowTarget(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera3D.FollowTarget");
            try {
                Quaternion orbitRotation = Quaternion.Euler(_orbitRotation.y, _orbitRotation.x, 0.0f);
                Quaternion lookRotation = Quaternion.Euler(_lookRotation.y, _lookRotation.x, 0.0f);

                // if we're going to try and return to default, including the target's rotation
                Quaternion finalOrbitRotation;
                if(_returnToDefault) {
                    Quaternion targetRotation = Quaternion.Euler(0.0f, Target.TargetTransform.eulerAngles.y, 0.0f);
                    finalOrbitRotation = targetRotation * orbitRotation;
                } else {
                    finalOrbitRotation = orbitRotation;
                }

                transform.rotation = finalOrbitRotation * lookRotation;

                // TODO: this doesn't work if we free-look and zoom
                // because we're essentially moving the target position, not the camera position
                // TODO: also enabling/disabling this causes the camera to zoom in and out, not sure why
                Vector3 velocity = Velocity;
                LastTargetPosition = Target.TargetTransform.position;
                LastTargetPosition = Smooth
                    ? Vector3.SmoothDamp(transform.position, LastTargetPosition, ref velocity, SmoothTime)
                    : LastTargetPosition;
                Velocity = velocity;

                transform.position = LastTargetPosition + finalOrbitRotation * new Vector3(0.0f, 0.0f, -_orbitMaxRadius);
            } finally {
                Profiler.EndSample();
            }
        }
    }
}
