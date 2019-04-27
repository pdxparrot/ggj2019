using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters
{
    public class CharacterMovement2D : ActorMovement2D
    {
        public CharacterBehavior2D CharacterBehavior2D => (CharacterBehavior2D)Behavior2D;

        public override bool UseGravity
        {
            get => base.UseGravity;
            set
            {
                base.UseGravity = value;
                if(!value) {
                    Velocity = Vector3.zero;
                }
            }
        }

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Behavior2D is CharacterBehavior2D);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            UseGravity = !IsKinematic && (!CharacterBehavior2D.IsGrounded || CharacterBehavior2D.IsMoving || CharacterBehavior2D.IsSliding);
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            /*Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity);*/

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);
        }
#endregion

        protected override void InitRigidbody(Rigidbody2D rb, ActorBehaviorData behaviorData)
        {
            base.InitRigidbody(rb, behaviorData);

            rb.isKinematic = behaviorData.IsKinematic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        public virtual void Jump(float height)
        {
            if(!CharacterBehavior2D.CanMove) {
                return;
            }

            // force physics to a sane state for the first frame of the jump
            UseGravity = true;
            CharacterBehavior2D.IsGrounded = false;

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + CharacterBehavior2D.CharacterBehaviorData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);
        }

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Velocity;
            if(CharacterBehavior2D.IsGrounded && !CharacterBehavior2D.IsMoving) {
                // prevent any weird ground adjustment shenanigans
                // when we're grounded and not moving
                adjustedVelocity.y = 0.0f;
            } else if(UseGravity) {
                // do some fudging to jumping/falling so it feels better
                float adjustment = CharacterBehavior2D.CharacterBehaviorData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -CharacterBehavior2D.CharacterBehaviorData.TerminalVelocity) {
                    adjustedVelocity.y = -CharacterBehavior2D.CharacterBehaviorData.TerminalVelocity;
                }
            }
            Velocity = adjustedVelocity;
        }
    }
}
