using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(CharacterActorController))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterActorController2D : ActorBehavior2D, ICharacterActorController
    {
        public CharacterActorControllerData ControllerData => _characterController.ControllerData;

        public Collider2D Collider => Owner2D.Collider;

        private CharacterActorController _characterController;

        public CharacterActorController CharacterController => _characterController;

        public float RaycastRoutineRate => _characterController.RaycastRoutineRate;

        public bool IsGrounded => _characterController.IsGrounded;

        public bool DidGroundCheckCollide => _characterController.DidGroundCheckCollide;

        public bool IsSliding => _characterController.IsSliding;

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

#if USE_SPINE
        [SerializeField]
        private SpineAnimationHelper _spineAnimation;

        protected SpineAnimationHelper SpineAnimation => _spineAnimation;
#endif

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private bool _useGravity = true;

        public virtual bool UseGravity
        {
            get => _useGravity;
            set
            {
                _useGravity = value;
                Velocity = Vector3.zero;
            }
        }

        public bool IsFalling => UseGravity && (!_characterController.IsGrounded && !_characterController.IsSliding && Velocity.y < 0.0f);
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _characterController = GetComponent<CharacterActorController>();
        }

        protected override void Update()
        {
            base.Update();

#if !USE_SPINE
            if(null != Animator) {
                Animator.SetBool(ControllerData.FallingParam, IsFalling);
            }
#endif
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            SetUseGravity(UseGravity && (!_characterController.IsGrounded || IsMoving || _characterController.IsSliding));
            _characterController.IsKinematic = IsKinematic;
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            /*Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity2D);*/

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);
        }
#endregion

        protected override void InitRigidbody(Rigidbody2D rb)
        {
            base.InitRigidbody(rb);

            rb.isKinematic = GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation2D.None;
        }

        public override void AnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(_characterController.RunOnComponents(c => c.OnAnimationMove(axes, dt))) {
                return;
            }

            DefaultAnimationMove(axes, dt);
        }

        public virtual void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

#if USE_SPINE
            SpineAnimation.SetFacing(LastMoveAxes);
#else
            // TODO: set facing (set localScale.x)
            if(null != Animator) {
                Animator.SetFloat(ControllerData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
                Animator.SetFloat(ControllerData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
            }
#endif
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            float speed = ControllerData.MoveSpeed;

            if(_characterController.RunOnComponents(c => c.OnPhysicsMove(axes, speed, dt))) {
                return;
            }

            if(!ControllerData.AllowAirControl && IsFalling) {
                return;
            }

            DefaultPhysicsMove(axes, speed, dt);
        }

        public virtual void DefaultPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(!CanMove) {
                return;
            }

            // TODO: handle slopes

            Vector3 velocity = axes * speed;
            if(!IsKinematic) {
                velocity.y = Velocity.y;
            }

            if(IsKinematic) {
                MovePosition(Position + velocity * dt);
            } else {
                Velocity = velocity;
            }
        }

        public virtual void Jump(float height, string animationParam)
        {
            if(!CanMove) {
                return;
            }

            // force physics to a sane state for the first frame of the jump
            _useGravity = true;
            _characterController.ResetGroundCheck();

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + ControllerData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);

#if !USE_SPINE
            if(null != Animator) {
                Animator.SetTrigger(animationParam);
            }
#endif
        }

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Velocity;
            if(_characterController.IsGrounded && !IsMoving) {
                // prevent any weird ground adjustment shenanigans
                // when we're grounded and not moving
                adjustedVelocity.y = 0.0f;
            } else if(UseGravity) {
                // do some fudging to jumping/falling so it feels better
                float adjustment = ControllerData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -ControllerData.TerminalVelocity) {
                    adjustedVelocity.y = -ControllerData.TerminalVelocity;
                }
            }
            Velocity = adjustedVelocity;
        }
    }
}