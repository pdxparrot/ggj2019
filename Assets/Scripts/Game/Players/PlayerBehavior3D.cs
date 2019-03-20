using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerBehavior3D : CharacterBehavior3D, IPlayerBehavior
    {
        public PlayerBehaviorData PlayerBehaviorData => (PlayerBehaviorData)BehaviorData;

        public IPlayer Player => (IPlayer)Owner;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(BehaviorData is PlayerBehaviorData);
            Assert.IsTrue(Owner is IPlayer);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // fixes sketchy rigidbody angular momentum shit
            AngularVelocity3D = Vector3.zero;
        }
#endregion

        public override void DefaultAnimationMove(Vector2 direction, float dt)
        {
            if(null == Player.Viewer) {
                base.DefaultAnimationMove(direction, dt);
                return;
            }

            // align with the camera
            Vector3 fixedDirection = new Vector3(direction.x, 0.0f, direction.y);
            Vector3 forward = (Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up) * fixedDirection).normalized;
            if(forward.sqrMagnitude > float.Epsilon) {
                Owner.transform.forward = forward;
            }

            if(null != Animator) {
                Animator.SetFloat(CharacterBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(direction.x) : 0.0f);
                Animator.SetFloat(CharacterBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(direction.y) : 0.0f);
            }
        }

        public override void DefaultPhysicsMove(Vector2 direction, float speed, float dt)
        {
            if(null == Player.Viewer) {
                base.DefaultPhysicsMove(direction, speed, dt);
                return;
            }

            Vector3 fixedDirection = new Vector3(direction.x, 0.0f, direction.y);
            Vector3 velocity = fixedDirection * speed;
            Quaternion rotation = Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up);
            velocity = rotation * velocity;

            if(IsKinematic) {
                MovePosition(Position + velocity * dt);
            } else {
                velocity.y = Velocity.y;
                Velocity = velocity;
            }
        }
    }
}
