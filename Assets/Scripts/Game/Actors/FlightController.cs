using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Players
{
    // TODO: create a FlightControllerData, similar to CharacterControllerData
    public class FlightController : ActorBehavior3D
    {
#region Physics
        [SerializeField]
        [ReadOnly]
        private Vector3 _bankForce;

        public Vector3 BankForce => _bankForce;

        public float Speed => CanMove ? 0.0f : (PartyParrotManager.Instance.IsPaused ? PauseState.Velocity.magnitude : Velocity.magnitude);

        public float Altitude => Owner.gameObject.transform.position.y;
#endregion

#region Movement Params
        [SerializeField]
        private float _maxAttackAngle = 45.0f;

        public float MaxAttackAngle => _maxAttackAngle;

        [SerializeField]
        private float _maxBankAngle = 45.0f;

        public float MaxBankAngle => _maxBankAngle;

        [SerializeField]
        private float _rotationAnimationSpeed = 5.0f;

        public float RotationAnimationSpeed => _rotationAnimationSpeed;

        [SerializeField]
        private float _linearThrust = 10.0f;

        public float LinearThrust => _linearThrust;

        [SerializeField]
        private float _turnSpeed = 10.0f;

        public float TurnSpeed => _turnSpeed;

        [SerializeField]
        private float _terminalVelocity = 10.0f;

        public float TerminalVelocity => _terminalVelocity;
#endregion

#region Unity Lifecycle
        protected override void Update()
        {
            base.Update();

#if DEBUG
            CheckForDebug();
#endif
        }

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity3D);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(Position, Position + _bankForce);
        }
#endregion

        protected override void InitRigidbody(Rigidbody rb)
        {
            base.InitRigidbody(rb);

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.freezeRotation = true;
            rb.detectCollisions = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation.None;
        }

        public void Redirect(Vector3 velocity)
        {
            Debug.Log($"Redirecting player {Owner.Id}: {velocity}");

            // unwind all of the rotations
            if(null != Owner.Model) {
                Owner.Model.transform.localRotation = Quaternion.Euler(0.0f, Owner.Model.transform.localEulerAngles.y, 0.0f);
            }
            Rotation3D = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);

            // stop moving
            Velocity = Vector3.zero;
            AngularVelocity3D = Vector3.zero;

            // move in an orderly fashion!
            Velocity = velocity;
        }

#region Input Handling
#if UNITY_EDITOR
        private void CheckForDebug()
        {
            if(Keyboard.current[Key.B].isPressed) {
                AngularVelocity3D = Vector3.zero;
                Velocity = Vector3.zero;
            }
        }
#endif
#endregion

        public override void AnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove || null == Owner.Model) {
                return;
            }

            Quaternion rotation = Owner.Model.transform.localRotation;
            Vector3 targetEuler = new Vector3
            {
                z = axes.x * -MaxBankAngle,
                x = axes.y * -MaxAttackAngle
            };

            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            rotation = Quaternion.Lerp(rotation, targetRotation, RotationAnimationSpeed * dt);

            Owner.Model.transform.localRotation = rotation;
        }

        private void Turn(Vector3 axes, float dt)
        {
#if true
            float turnSpeed = TurnSpeed * axes.x;
            Quaternion rotation = Quaternion.AngleAxis(turnSpeed * dt, Vector3.up);
            MoveRotation(Rotation3D * rotation);
#else
            // TODO: this only works if Y rotation is unconstrained
            // it also breaks because the model rotates :(
            const float AngularThrust = 0.5f;
            AddRelativeTorque(Vector3.up * AngularThrust * axes.x);
#endif

            // adding a force opposite our current x velocity should help stop us drifting
            Vector3 relativeVelocity = transform.InverseTransformDirection(Velocity);
            _bankForce = -relativeVelocity.x * AngularDrag * transform.right;
            AddForce(_bankForce);
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            Turn(axes, dt);

            float attackAngle = axes.y * -MaxAttackAngle;
            Vector3 attackVector = Quaternion.AngleAxis(attackAngle, Vector3.right) * Vector3.forward;
            AddRelativeForce(attackVector * LinearThrust);

            // lift if we're not falling
            if(axes.y >= 0.0f) {
                AddForce(-Physics.gravity, ForceMode.Acceleration);
            }

            // cap our fall speed
            Vector3 velocity = Velocity;
            if(velocity.y < -TerminalVelocity) {
                Velocity = new Vector3(velocity.x, -TerminalVelocity, velocity.z);
            }
        }
    }
}
