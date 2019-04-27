using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players
{
    public abstract class PlayerBehavior3D : CharacterBehavior3D, IPlayerBehavior
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

        protected override void Update()
        {
            base.Update();

            IsMoving = MoveDirection.sqrMagnitude > 0.001f;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // fixes sketchy rigidbody angular momentum shit
            Movement3D.AngularVelocity = Vector3.zero;
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
            if(!CanMove) {
                return;
            }

            Vector3 fixedDirection = new Vector3(MoveDirection.x, 0.0f, MoveDirection.y);
            Vector3 forward = fixedDirection;
            if(null != Player.Viewer) {
                // align with the camera instead of the movement
                forward = (Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up) * fixedDirection).normalized;
                if(forward.sqrMagnitude > float.Epsilon) {
                    Owner.transform.forward = forward;
                }
            }

            if(forward.sqrMagnitude > float.Epsilon) {
                Owner.transform.forward = forward;
            }

            if(null != Animator) {
                Animator.SetFloat(PlayerBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(MoveDirection.x) : 0.0f);
                Animator.SetFloat(PlayerBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(MoveDirection.y) : 0.0f);
            }

            base.AnimationUpdate(dt);
        }

        protected override void PhysicsUpdate(float dt)
        {
            if(!CanMove) {
                return;
            }

            if(!PlayerBehaviorData.AllowAirControl && IsFalling) {
                return;
            }

            Vector3 fixedDirection = new Vector3(MoveDirection.x, 0.0f, MoveDirection.y);
            Vector3 velocity = fixedDirection * PlayerBehaviorData.MoveSpeed;
            Quaternion rotation = Movement3D.Rotation;
            if(null != Player.Viewer) {
                // rotate with the camera instead of the movement
                rotation = Quaternion.AngleAxis(Player.Viewer.transform.localEulerAngles.y, Vector3.up);
            }
            velocity = rotation * velocity;

            if(Movement3D.IsKinematic) {
                Movement3D.Teleport(Movement3D.Position + velocity * dt);
            } else {
                velocity.y = Movement3D.Velocity.y;
                Movement3D.Velocity = velocity;
            }

            base.PhysicsUpdate(dt);
        }
    }
}
