using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Actor2D : Actor
    {
#region Collider
        public Collider2D Collider { get; private set; }
#endregion

        public ActorBehavior2D Behavior2D => (ActorBehavior2D)Behavior;

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is ActorBehavior2D);

            Collider = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Behavior2D.CollisionEnter(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Behavior2D.CollisionStay(collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Behavior2D.CollisionExit(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Behavior2D.TriggerEnter(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Behavior2D.TriggerStay(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Behavior2D.TriggerExit(other.gameObject);
        }
#endregion
    }
}
