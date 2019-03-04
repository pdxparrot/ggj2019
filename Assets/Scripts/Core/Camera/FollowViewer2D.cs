using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(FollowCamera2D))]
    public class FollowViewer2D : FollowViewer
    {
        public FollowCamera2D FollowCamera2D => (FollowCamera2D)FollowCamera;

        private Rigidbody2D _rigidbody;

        // TODO: need more work on this
        //private Collider2D _collider;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(FollowCamera is FollowCamera2D);

            _rigidbody = GetComponent<Rigidbody2D>();
            InitRigidbody();

            //_collider = GetComponent<Collider2D>();
            //_collider.isTrigger = true;

            Set2D();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_rigidbody.isKinematic) {
                return;
            }

            Debug.LogWarning("TODO: FollowViewer2D collision!");
        }
#endregion

        private void InitRigidbody()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            _rigidbody.interpolation = RigidbodyInterpolation2D.None;
        }
    }
}
