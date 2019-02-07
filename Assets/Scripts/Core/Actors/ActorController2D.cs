using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    // TODO: rename ActorBehavior2D
    public class ActorController2D : ActorController
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;

            public void Save(Rigidbody2D rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody2D rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
            }
        }

        public Actor2D Owner2D => (Actor2D)Owner;

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private float _lastAngularVelocity;

        [SerializeField]
        private Rigidbody2D _rigidbody;

        public override Vector3 Position
        {
            get => _rigidbody.position;
            set
            {
                Debug.Log($"Teleporting actor {Owner.Id} to {value}");
                _rigidbody.position = value;
            }
        }

        public override Quaternion Rotation3D
        {
            get => Quaternion.identity;
            set {}
        }

        public override float Rotation2D
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public override Vector3 AngularVelocity3D
        {
            get => Vector3.zero;
            set {}
        }

        public override float AngularVelocity2D
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
            base.Awake();

            Assert.IsTrue(Owner is Actor2D);

            InitRigidbody(_rigidbody);
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = _rigidbody.velocity;
            _lastAngularVelocity = _rigidbody.angularVelocity;
        }
#endregion

        protected virtual void InitRigidbody(Rigidbody2D rb)
        {
        }

        protected void SetUseGravity(bool useGravity)
        {
            _rigidbody.gravityScale = useGravity ? 1.0f : 0.0f;
        }

        public void MovePosition(Vector3 position)
        {
            //Debug.Log($"Teleporting actor {Owner.Id} to {position} (interpolated)");

            _rigidbody.MovePosition(position);
        }

        public void MoveRotation(float angle)
        {
            _rigidbody.MoveRotation(angle);
        }

        // TODO: this can go away when ICharacterActorController goes away
        public void AddForce(Vector3 force)
        {
            AddForce(force, ForceMode2D.Force);
        }

        public void AddForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddForce(force, mode);
        }

        public void AddRelativeForce(Vector3 force, ForceMode2D mode)
        {
            _rigidbody.AddRelativeForce(force, mode);
        }

        public void AddTorque(float torque, ForceMode2D mode=ForceMode2D.Force)
        {
            _rigidbody.AddTorque(torque, mode);
        }

#region Event Handlers
        protected override void PauseEventHandler(object sender, EventArgs args)
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
