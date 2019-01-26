using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(CharacterActorController))]
    [RequireComponent(typeof(Collider2D))]
    public class CharacterActorController2D : ActorController2D, ICharacterActorController
    {
        public CharacterActorControllerData ControllerData => _characterController.ControllerData;

        public Collider2D Collider => PhysicsOwner.Collider;

        private CharacterActorController _characterController;

        public float RaycastRoutineRate => _characterController.RaycastRoutineRate;

        public bool IsGrounded => _characterController.IsGrounded;

        public bool DidGroundCheckCollide => _characterController.DidGroundCheckCollide;

        public bool IsSliding => _characterController.IsSliding;

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

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
                Rigidbody.velocity = Vector3.zero;
            }
        }

        public bool IsFalling => UseGravity && (!_characterController.IsGrounded && !_characterController.IsSliding && Rigidbody.velocity.y < 0.0f);
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _characterController = GetComponent<CharacterActorController>();

            InitRigidbody();
        }

        protected override void Update()
        {
            base.Update();

            if(null != Owner.Animator) {
                Owner.Animator.SetBool(ControllerData.FallingParam, IsFalling);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            _characterController.IsKinematic = Rigidbody.isKinematic;
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            /*Gizmos.color = Color.green;
            Gizmos.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.angularVelocity);*/

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.velocity);
        }
#endregion

        private void InitRigidbody()
        {
            Rigidbody.isKinematic = GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            Rigidbody.interpolation = RigidbodyInterpolation2D.None;
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

        public void DefaultAnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            // TODO: do we need to rotate to face the direction of movement?

            if(null != Owner.Animator) {
                Owner.Animator.SetFloat(ControllerData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
                Owner.Animator.SetFloat(ControllerData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
            }
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(_characterController.RunOnComponents(c => c.OnPhysicsMove(axes, dt))) {
                return;
            }

            if(!ControllerData.AllowAirControl && IsFalling) {
                return;
            }

            DefaultPhysicsMove(axes, ControllerData.MoveSpeed, dt);
        }

        public void DefaultPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(!CanMove) {
                return;
            }

            Vector3 fixedAxes = new Vector3(axes.x, axes.y, 0.0f);

            // TODO: handle slopes

            Vector2 velocity = fixedAxes * speed;
            if(!Rigidbody.isKinematic) {
                velocity.y = Rigidbody.velocity.y;
            }

            if(Rigidbody.isKinematic) {
                Rigidbody.MovePosition(Rigidbody.position + velocity * dt);
            } else {
                Rigidbody.velocity = velocity;
            }
        }

        public void Jump(float height, string animationParam)
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
            Rigidbody.velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);

            if(null != Owner.Animator) {
                Owner.Animator.SetTrigger(animationParam);
            }
        }

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Rigidbody.velocity;
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
            Rigidbody.velocity = adjustedVelocity;
        }
    }
}