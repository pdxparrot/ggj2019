using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerFlightBehavior3D : CharacterFlightBehavior3D, IPlayerBehavior
    {
        [SerializeField]
        [ReadOnly]
        private Vector2 _moveDirection;

        public Vector2 MoveDirection => _moveDirection;

        public PlayerBehaviorData PlayerBehaviorData => (PlayerBehaviorData)BehaviorData;

        public IPlayer Player => (IPlayer)Owner;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(BehaviorData is PlayerBehaviorData);
            Assert.IsTrue(Owner is IPlayer);
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            base.Initialize(behaviorData);

            _moveDirection = Vector2.zero;
        }

        public void SetMoveDirection(Vector2 moveDirection)
        {
            _moveDirection = Vector2.ClampMagnitude(moveDirection, 1.0f);
        }

        protected override void AnimationUpdate(float dt)
        {
            if(!CanMove || null == Owner.Model) {
                return;
            }

            Transform modelTransform = Owner.Model.transform;

            Quaternion rotation = modelTransform.localRotation;
            Vector3 targetEuler = new Vector3
            {
                z = MoveDirection.x * -FlightBehaviorData.MaxBankAngle,
                x = MoveDirection.y * -FlightBehaviorData.MaxAttackAngle
            };

            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            rotation = Quaternion.Lerp(rotation, targetRotation, FlightBehaviorData.RotationAnimationSpeed * dt);

            modelTransform.localRotation = rotation;

            base.AnimationUpdate(dt);
        }

        protected override void PhysicsUpdate(float dt)
        {
            if(!CanMove) {
                return;
            }

            Turn(MoveDirection, dt);

            float attackAngle = MoveDirection.y * -FlightBehaviorData.MaxAttackAngle;
            Vector3 attackVector = Quaternion.AngleAxis(attackAngle, Vector3.right) * Vector3.forward;
            AddRelativeForce(attackVector * FlightBehaviorData.LinearThrust, ForceMode.Force);

            // lift if we're not falling
            if(MoveDirection.y >= 0.0f) {
                AddForce(-Physics.gravity, ForceMode.Acceleration);
            }

            // cap our fall speed
            Vector3 velocity = Velocity;
            if(velocity.y < -FlightBehaviorData.TerminalVelocity) {
                Velocity = new Vector3(velocity.x, -FlightBehaviorData.TerminalVelocity, velocity.z);
            }

            base.PhysicsUpdate(dt);
        }
    }
}
