using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters
{
    public class CharacterMovement3D : ActorMovement3D
    {
        public CharacterBehavior3D CharacterBehavior3D => (CharacterBehavior3D)Behavior3D;

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
            Assert.IsTrue(Behavior3D is CharacterBehavior3D);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            UseGravity = !IsKinematic && (!CharacterBehavior3D.IsGrounded || CharacterBehavior3D.IsMoving || CharacterBehavior3D.IsSliding);
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);
        }
#endregion

        protected override void InitRigidbody(Rigidbody rb, ActorBehaviorData behaviorData)
        {
            base.InitRigidbody(rb, behaviorData);

            rb.isKinematic = behaviorData.IsKinematic;
            rb.useGravity = !behaviorData.IsKinematic;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.detectCollisions = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public virtual void Jump(float height)
        {
            if(!CharacterBehavior3D.CanMove) {
                return;
            }

            // force physics to a sane state for the first frame of the jump
            UseGravity = true;
            CharacterBehavior3D.IsGrounded = false;

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + CharacterBehavior3D.CharacterBehaviorData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);
        }

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Velocity;
            if(CharacterBehavior3D.IsGrounded && !CharacterBehavior3D.IsMoving) {
                // prevent any weird ground adjustment shenanigans
                // when we're grounded and not moving
                adjustedVelocity.y = 0.0f;
            } else if(UseGravity) {
                // do some fudging to jumping/falling so it feels better
                float adjustment = CharacterBehavior3D.CharacterBehaviorData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -CharacterBehavior3D.CharacterBehaviorData.TerminalVelocity) {
                    adjustedVelocity.y = -CharacterBehavior3D.CharacterBehaviorData.TerminalVelocity;
                }
            }
            Velocity = adjustedVelocity;
        }
    }
}
