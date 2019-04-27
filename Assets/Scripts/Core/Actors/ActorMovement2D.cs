using System;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    public class ActorMovement2D : ActorMovement
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;
            public float AngularVelocity;

            public void Save(Rigidbody2D rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;

                AngularVelocity = rigidbody.angularVelocity;
                rigidbody.angularVelocity = 0.0f;
            }

            public void Restore(Rigidbody2D rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
                rigidbody.angularVelocity = AngularVelocity;
            }
        }

        public ActorBehavior2D Behavior2D => (ActorBehavior2D)Behavior;

        [Space(10)]

#region Physics
        [Header("Physics")]

        // expose some useful rigidbody properties that unity doesn't
        [SerializeField]
        [ReadOnly]
        private float _lastGravityScale;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        public override Vector3 Position
        {
            get => _rigidbody.position;
            set
            {
                Debug.Log($"Teleporting actor {Behavior2D.Owner2D.Id} to {value}");
                _rigidbody.position = value;
            }
        }

        public float Rotation
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public float AngularVelocity
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
            get => _rigidbody.gravityScale > 0.0f;
            set => _rigidbody.gravityScale = value ? 1.0f : 0.0f;
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
            Assert.IsTrue(Behavior is ActorBehavior2D);

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
            _lastGravityScale = _rigidbody.gravityScale;
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            base.Initialize(behaviorData);

            Rotation = 0.0f;

            AngularVelocity = 0.0f;

            InitRigidbody(_rigidbody, behaviorData);
        }

        protected virtual void InitRigidbody(Rigidbody2D rb, ActorBehaviorData behaviorData)
        {
        }

        public override void Teleport(Vector3 position)
        {
            //Debug.Log($"Teleporting actor {Behavior2D.Owner2D.Id} to {position} (interpolated)");
            _rigidbody.MovePosition(position);
        }

        public override void MoveTowards(Vector3 position, float speed, float dt)
        {
            Vector3 newPosition = Vector3.MoveTowards(Position, position, speed * dt);
            _rigidbody.MovePosition(newPosition);
        }

        public void MoveRotation(float angle)
        {
            _rigidbody.MoveRotation(angle);
        }

        public void AddForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddRelativeForce(force, mode);
        }

        public void AddTorque(float torque, ForceMode2D mode)
        {
            _rigidbody.AddTorque(torque, mode);
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
