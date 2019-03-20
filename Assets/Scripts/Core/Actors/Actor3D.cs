using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class Actor3D : Actor
    {
#region Collider
        public Collider Collider { get; private set; }
#endregion

        public ActorBehavior3D Behavior3D => (ActorBehavior3D)Behavior;

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is ActorBehavior3D);

            Collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Behavior3D.CollisionEnter(collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            Behavior3D.CollisionStay(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            Behavior3D.CollisionExit(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Behavior3D.TriggerEnter(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            Behavior3D.TriggerStay(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            Behavior3D.TriggerExit(other.gameObject);
        }
#endregion
    }
}
