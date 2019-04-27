﻿using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players
{
    public abstract class PlayerFlightBehavior3D : CharacterBehavior3D, IPlayerBehavior
    {
        private CharacterFlightMovement3D CharacterFlightMovement3D => (CharacterFlightMovement3D)Movement3D;

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
            Assert.IsTrue(Movement3D is CharacterFlightMovement3D);
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
                z = MoveDirection.x * -CharacterFlightMovement3D.FlightMovementData.MaxBankAngle,
                x = MoveDirection.y * -CharacterFlightMovement3D.FlightMovementData.MaxAttackAngle
            };

            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            rotation = Quaternion.Lerp(rotation, targetRotation, CharacterFlightMovement3D.FlightMovementData.RotationAnimationSpeed * dt);

            modelTransform.localRotation = rotation;

            base.AnimationUpdate(dt);
        }

        protected override void PhysicsUpdate(float dt)
        {
            if(!CanMove) {
                return;
            }

            CharacterFlightMovement3D.Turn(MoveDirection, dt);

            float attackAngle = MoveDirection.y * -CharacterFlightMovement3D.FlightMovementData.MaxAttackAngle;
            Vector3 attackVector = Quaternion.AngleAxis(attackAngle, Vector3.right) * Vector3.forward;
            CharacterFlightMovement3D.AddRelativeForce(attackVector * CharacterFlightMovement3D.FlightMovementData.LinearThrust, ForceMode.Force);

            // lift if we're not falling
            if(MoveDirection.y >= 0.0f) {
                CharacterFlightMovement3D.AddForce(-Physics.gravity, ForceMode.Acceleration);
            }

            // cap our fall speed
            Vector3 velocity = CharacterFlightMovement3D.Velocity;
            if(velocity.y < -CharacterFlightMovement3D.FlightMovementData.TerminalVelocity) {
                CharacterFlightMovement3D.Velocity = new Vector3(velocity.x, -CharacterFlightMovement3D.FlightMovementData.TerminalVelocity, velocity.z);
            }

            base.PhysicsUpdate(dt);
        }
    }
}
