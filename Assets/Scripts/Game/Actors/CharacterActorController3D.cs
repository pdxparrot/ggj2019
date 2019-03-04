using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors
{
    // TODO: merge this into ActorBehavior
    // TODO: this shouldn't care about USE_SPINE being set
    [RequireComponent(typeof(CharacterActorController))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterActorController3D : ActorBehavior3D, ICharacterActorController
    {
        public CharacterActorControllerData ControllerData => _characterController.ControllerData;

        public CapsuleCollider Capsule => (CapsuleCollider)Owner3D.Collider;

        private CharacterActorController _characterController;

        public CharacterActorController CharacterController => _characterController;

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
                if(!_useGravity) {
                    Velocity = Vector3.zero;
                }
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

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity3D);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);
        }
#endregion

        protected override void InitRigidbody(Rigidbody rb)
        {
            base.InitRigidbody(rb);

            rb.isKinematic = GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic;
            rb.useGravity = !GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.detectCollisions = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation.None;
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

            Vector3 forward = new Vector3(axes.x, 0.0f, axes.y);

            // align the movement with the camera
            if(null != Owner.Viewer) {
                forward = (Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) * forward).normalized;
            }

            // align the player with the movement
            if(forward.sqrMagnitude > float.Epsilon) {
                transform.forward = forward;
            }

#if !USE_SPINE
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

            Vector3 fixedAxes = new Vector3(axes.x, 0.0f, axes.y);

            // prevent moving up slopes we can't move up
            if(_characterController.GroundSlope >= ControllerData.SlopeLimit) {
                float dp = Vector3.Dot(transform.forward, _characterController.GroundCheckNormal);
                if(dp < 0.0f) {
                    fixedAxes.z = 0.0f;
                }
            }

            Vector3 velocity = fixedAxes * speed;
            Quaternion rotation = null != Owner.Viewer ? Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) : transform.rotation;
            velocity = rotation * velocity;
            velocity.y = Velocity.y;

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
