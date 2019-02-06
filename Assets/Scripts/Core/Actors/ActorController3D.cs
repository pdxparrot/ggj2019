using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: reduce the copy paste in this
    // TODO: rename ActorBehavior3D
    // TODO: hook the rigidbody rather than requiring it
    [RequireComponent(typeof(Rigidbody))]
    public class ActorController3D : ActorController
    {
        [Serializable]
        protected struct InternalPauseState
        {
            public bool IsKinematic;
            public Vector3 Velocity;

            public void Save(Rigidbody rigidbody)
            {
                IsKinematic = rigidbody.isKinematic;
                rigidbody.isKinematic = true;

                Velocity = rigidbody.velocity;
                rigidbody.velocity = Vector3.zero;
            }

            public void Restore(Rigidbody rigidbody)
            {
                rigidbody.isKinematic = IsKinematic;
                rigidbody.velocity = Velocity;
            }
        }

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastAngularVelocity;

        public Actor3D Owner3D => (Actor3D)Owner;

        public Rigidbody Rigidbody { get; private set; }

        public override Vector3 Position
        {
            get => Rigidbody.position;
            set
            {
                Debug.Log($"Teleporting actor {Owner.Id} to {value}");
                Rigidbody.position = value;
            }
        }

        public override Quaternion Rotation3D
        {
            get => Rigidbody.rotation;
            set => Rigidbody.rotation = value;
        }

        public override float Rotation2D
        {
            get => 0.0f;
            set {}
        }

        public override Vector3 Velocity
        {
            get => Rigidbody.velocity;
            set => Rigidbody.velocity = value;
        }

        public override float Mass
        {
            get => Rigidbody.mass;
            set => Rigidbody.mass = value;
        }

        public override float LinearDrag
        {
            get => Rigidbody.drag;
            set => Rigidbody.drag = value;
        }

        public override float AngularDrag
        {
            get => Rigidbody.angularDrag;
            set => Rigidbody.angularDrag = value;
        }

        public override bool IsKinematic
        {
            get => Rigidbody.isKinematic;
            set => Rigidbody.isKinematic = value;
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

            Assert.IsTrue(Owner is Actor3D);

            Rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void LateUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
            _lastAngularVelocity = Rigidbody.angularVelocity;
        }
#endregion

        public void MovePosition(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position} (interpolated)");

            Rigidbody.MovePosition(position);
        }

        public void AddForce(Vector3 force)
        {
            Rigidbody.AddForce(force, ForceMode.Force);
        }

#region Event Handlers
        protected override void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                _pauseState.Save(Rigidbody);
            } else {
                _pauseState.Restore(Rigidbody);
            }
        }
#endregion
    }
}
