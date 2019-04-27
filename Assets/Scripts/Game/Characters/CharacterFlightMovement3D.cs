using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Characters
{
    public abstract class CharacterFlightMovement3D : ActorMovement3D
    {
        public CharacterBehavior3D CharacterBehavior3D => (CharacterBehavior3D)Behavior3D;

        [SerializeField]
        private CharacterFlightMovementData _data;

        public CharacterFlightMovementData FlightMovementData => _data;

#region Physics
        [SerializeField]
        [ReadOnly]
        private Vector3 _bankForce;

        public Vector3 BankForce => _bankForce;

        public float Speed => CharacterBehavior3D.CanMove ? 0.0f : (PartyParrotManager.Instance.IsPaused ? PauseState.Velocity.magnitude : Velocity.magnitude);

        public float Altitude => Position.y;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsNotNull(_data);

            base.Awake();
        }

        private void Update()
        {
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
            Gizmos.DrawLine(Position, Position + AngularVelocity);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(Position, Position + _bankForce);
        }
#endregion

        protected override void InitRigidbody(Rigidbody rb, ActorBehaviorData behaviorData)
        {
            base.InitRigidbody(rb, behaviorData);

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
            Debug.Log($"Redirecting player {CharacterBehavior3D.Owner3D.Id}: {velocity}");

            // unwind all of the rotations
            if(null != CharacterBehavior3D.Owner3D.Model) {
                Transform modelTransform = CharacterBehavior3D.Owner3D.Model.transform;
                modelTransform.localRotation = Quaternion.Euler(0.0f, modelTransform.localEulerAngles.y, 0.0f);
            }
            Rotation = Quaternion.Euler(0.0f, CharacterBehavior3D.Owner.transform.eulerAngles.y, 0.0f);

            // stop moving
            Velocity = Vector3.zero;
            AngularVelocity = Vector3.zero;

            // move in an orderly fashion!
            Velocity = velocity;
        }

#region Input Handling
#if UNITY_EDITOR
        private void CheckForDebug()
        {
            if(Keyboard.current[Key.B].isPressed) {
                AngularVelocity = Vector3.zero;
                Velocity = Vector3.zero;
            }
        }
#endif
#endregion

        public void Turn(Vector2 direction, float dt)
        {
#if true
            float turnSpeed = _data.TurnSpeed * direction.x;
            Quaternion rotation = Quaternion.AngleAxis(turnSpeed * dt, Vector3.up);
            MoveRotation(Rotation * rotation);
#else
            // TODO: this only works if Y rotation is unconstrained
            // it also breaks because the model rotates :(
            const float AngularThrust = 0.5f;
            AddRelativeTorque(Vector3.up * AngularThrust * direction.x, ForceMode.Force);
#endif

            Transform ownerTransform = CharacterBehavior3D.Owner3D.transform;

            // adding a force opposite our current x velocity should help stop us drifting
            Vector3 relativeVelocity = ownerTransform.InverseTransformDirection(Velocity);
            _bankForce = -relativeVelocity.x * AngularDrag * ownerTransform.right;
            AddForce(_bankForce, ForceMode.Force);
        }
    }
}
