using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players
{
    public abstract class PlayerBehavior2D : CharacterBehavior2D, IPlayerBehavior
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
            AngularVelocity2D = 0.0f;
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            base.Initialize(behaviorData);

            _moveDirection = Vector3.zero;
        }

        protected override void InitRigidbody(Rigidbody2D rb)
        {
            base.InitRigidbody(rb);

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation2D.None;
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

            // align with the movement
#if USE_SPINE
            if(IsMoving && null != AnimationHelper) {
                AnimationHelper.SetFacing(MoveDirection);
            }
#else
            // TODO: set facing (set localScale.x)
            if(null != Animator) {
                Animator.SetFloat(PlayerBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(MoveDirection.x) : 0.0f);
                Animator.SetFloat(PlayerBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(MoveDirection.y) : 0.0f);
            }
#endif

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

            Vector3 velocity = MoveDirection * PlayerBehaviorData.MoveSpeed;
            if(IsKinematic) {
                Teleport(Position + velocity * dt);
            } else {
                velocity.y = Velocity.y;
                Velocity = velocity;
            }

            base.PhysicsUpdate(dt);
        }
    }
}
