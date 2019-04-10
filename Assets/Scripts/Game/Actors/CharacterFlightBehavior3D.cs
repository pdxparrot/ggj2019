using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    // TODO: can this be a component?
    public abstract class CharacterFlightBehavior3D : ActorBehavior3D
    {
        public CharacterBehaviorData CharacterBehaviorData => (CharacterBehaviorData)BehaviorData;

        [SerializeField]
        private CharacterFlightBehaviorData _data;

        protected CharacterFlightBehaviorData FlightBehaviorData => _data;

#region Physics
        [SerializeField]
        [ReadOnly]
        private Vector3 _bankForce;

        public Vector3 BankForce => _bankForce;

        public float Speed => CanMove ? 0.0f : (PartyParrotManager.Instance.IsPaused ? PauseState.Velocity.magnitude : Velocity.magnitude);

        public float Altitude => Position.y;
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

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is CharacterFlightBehaviorData);

            base.Initialize(behaviorData);
        }

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
                Transform modelTransform = Owner.Model.transform;
                modelTransform.localRotation = Quaternion.Euler(0.0f, modelTransform.localEulerAngles.y, 0.0f);
            }
            Rotation3D = Quaternion.Euler(0.0f, Owner.transform.eulerAngles.y, 0.0f);

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

        protected void Turn(Vector2 direction, float dt)
        {
#if true
            float turnSpeed = _data.TurnSpeed * direction.x;
            Quaternion rotation = Quaternion.AngleAxis(turnSpeed * dt, Vector3.up);
            MoveRotation(Rotation3D * rotation);
#else
            // TODO: this only works if Y rotation is unconstrained
            // it also breaks because the model rotates :(
            const float AngularThrust = 0.5f;
            AddRelativeTorque(Vector3.up * AngularThrust * direction.x, ForceMode.Force);
#endif

            Transform ownerTransform = Owner.transform;

            // adding a force opposite our current x velocity should help stop us drifting
            Vector3 relativeVelocity = ownerTransform.InverseTransformDirection(Velocity);
            _bankForce = -relativeVelocity.x * AngularDrag * ownerTransform.right;
            AddForce(_bankForce, ForceMode.Force);
        }
    }
}
