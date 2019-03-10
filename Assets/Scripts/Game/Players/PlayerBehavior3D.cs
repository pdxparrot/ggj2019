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

        public override void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(null == Player.Viewer) {
                base.DefaultAnimationMove(axes, dt);
                return;
            }

            if(!CanMove) {
                return;
            }

            // align with the camera
            Vector3 fixedAxes = new Vector3(axes.x, 0.0f, axes.y);
            Vector3 forward = (Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up) * fixedAxes).normalized;
            if(forward.sqrMagnitude > float.Epsilon) {
                Owner.transform.forward = forward;
            }

            if(null != Animator) {
                Animator.SetFloat(CharacterBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
                Animator.SetFloat(CharacterBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
            }
        }

        public override void DefaultPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(null == Player.Viewer) {
                base.DefaultPhysicsMove(axes, speed, dt);
                return;
            }

            if(!CanMove) {
                return;
            }

            Vector3 velocity = axes * speed;
            Quaternion rotation = Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up);
            velocity = rotation * velocity;
            velocity.y = Velocity.y;

            if(IsKinematic) {
                MovePosition(Position + velocity * dt);
            } else {
                Velocity = velocity;
            }
        }
    }
}
