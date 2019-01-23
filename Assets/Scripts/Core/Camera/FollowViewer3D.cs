using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(FollowCamera3D))]
    public class FollowViewer3D : FollowViewer
    {
        public FollowCamera3D FollowCamera3D => (FollowCamera3D)FollowCamera;

        private Rigidbody _rigidbody;

        // TODO: need more work on this
        //private Collider _collider;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(FollowCamera is FollowCamera3D);

            _rigidbody = GetComponent<Rigidbody>();
            InitRigidbody();

            //_collider = GetComponent<Collider>();
            //_collider.isTrigger = true;

            Set3D();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_rigidbody.isKinematic) {
                return;
            }

            Debug.LogWarning("TODO: FollowViewer3D collision!");
        }
#endregion

        private void InitRigidbody()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.detectCollisions = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }
    }
}
