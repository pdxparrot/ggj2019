using System;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    public class ActorMovement3D : ActorMovement
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;

            public void Save(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;

                AngularVelocity = rigidbody.angularVelocity;
                rigidbody.angularVelocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
        }

        public ActorBehavior3D Behavior3D => (ActorBehavior3D)Behavior;

        [Space(10)]

#region Physics
        [Header("Physics")]

        // expose some useful rigidbody properties that unity doesn't
        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;

        [SerializeField]
        private Rigidbody _rigidbody;

        public override Vector3 Position
        {
            get => _rigidbody.position;
            set
            {
                Debug.Log($"Teleporting actor {Behavior3D.Owner3D.Id} to {value}");
                _rigidbody.position = value;
            }
        }

        public Quaternion Rotation
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public Vector3 AngularVelocity
        {
            get => _rigidbody.angularVelocity;
            set => _rigidbody.angularVelocity = value;
        }

        public override float Mass
        {
            get => _rigidbody.mass;
            set => _rigidbody.mass = value;
        }

        public override float LinearDrag
        {
            get => _rigidbody.drag;
            set => _rigidbody.drag = value;
        }

        public override float AngularDrag
        {
            get => _rigidbody.angularDrag;
            set => _rigidbody.angularDrag = value;
        }

        public override bool IsKinematic
        {
            get => _rigidbody.isKinematic;
            set => _rigidbody.isKinematic = value;
        }

        public override bool UseGravity
        {
            get => _rigidbody.useGravity;
            set => _rigidbody.useGravity = value;
        }
#endregion

        [Space(10)]

#region Pause State
        [Header("Pause State")]

        [SerializeField]
        [ReadOnly]
        private InternalPauseState _pauseState;

        protected InternalPauseState PauseState => _pauseState;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Behavior is ActorBehavior3D);

            base.Awake();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = _rigidbody.velocity;
            _lastAngularVelocity = _rigidbody.angularVelocity;
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            base.Initialize(behaviorData);

            Rotation = Quaternion.identity;

            AngularVelocity = Vector3.zero;

            InitRigidbody(_rigidbody, behaviorData);
        }

        protected virtual void InitRigidbody(Rigidbody rb, ActorBehaviorData behaviorData)
        {
        }

        public override void Teleport(Vector3 position)
        {
            //Debug.Log($"Teleporting actor {Behavior3D.Owner3D.Id} to {position} (interpolated)");
            _rigidbody.MovePosition(position);
        }

        public override void MoveTowards(Vector3 position, float speed, float dt)
        {
            Vector3 newPosition = Vector3.MoveTowards(Position, position, speed * dt);
            _rigidbody.MovePosition(newPosition);
        }

        public void MoveRotation(Quaternion rot)
        {
            _rigidbody.MoveRotation(rot);
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode)
        {
            _rigidbody.AddRelativeForce(force, mode);
        }

        public void AddTorque(Vector3 torque, ForceMode mode)
        {
            _rigidbody.AddTorque(torque, mode);
        }

        public void AddRelativeTorque(Vector3 torque, ForceMode mode)
        {
            _rigidbody.AddRelativeTorque(torque, mode);
        }

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(_rigidbody);
            } else {
                _pauseState.Restore(_rigidbody);
            }
        }
#endregion
    }
}
