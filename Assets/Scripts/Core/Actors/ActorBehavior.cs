using JetBrains.Annotations;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorBehavior : MonoBehaviour
    {
        [SerializeField]
        private Actor _owner;

        public Actor Owner => _owner;

        [SerializeField]
        [ReadOnly]
        protected ActorBehaviorData _behaviorData;

        public ActorBehaviorData BehaviorData => _behaviorData;

        //[Space(10)]

#region Movement
        //[Header("Movement")]

        public bool IsMoving { get; protected set; }

        public abstract bool CanMove { get; }
#endregion

        [Space(10)]

#region Effects
        [Header("Actor Effects")]

        [SerializeField]
        [CanBeNull]
        protected EffectTrigger _spawnEffect;

        [SerializeField]
        [CanBeNull]
        protected EffectTrigger _respawnEffect;

        [SerializeField]
        [CanBeNull]
        protected EffectTrigger _despawnEffect;
#endregion

        //[Space(10)]

#region Physics
        //[Header("Physics")]

        private Transform _transform;

        public virtual Vector3 Position
        {
            get => _transform.position;
            set
            {
                Debug.Log($"Teleporting actor {Owner.Id} to {value}");
                _transform.position = value;
            }
        }

        public virtual Quaternion Rotation3D
        {
            get => _transform.rotation;
            set => _transform.rotation = value;
        }

        public virtual float Rotation2D
        {
            get => 0.0f;
            set {}
        }

        public virtual Vector3 Velocity
        {
            get => Vector3.zero;
            set {}
        }

        public virtual Vector3 AngularVelocity3D
        {
            get => Vector3.zero;
            set {}
        }

        public virtual float AngularVelocity2D
        {
            get => 0.0f;
            set {}
        }

        public virtual float Mass
        {
            get => 1.0f;
            set {}
        }

        public virtual float LinearDrag
        {
            get => 0.0f;
            set {}
        }

        public virtual float AngularDrag
        {
            get => 0.0f;
            set {}
        }

        public virtual bool IsKinematic
        {
            get => true;
            set {}
        }

        public virtual bool UseGravity
        {
            get => false;
            set {}
        }
#endregion

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsNotNull(Owner);

            _transform = Owner.GetComponent<Transform>();

            // always start out kinematic so that we don't
            // fall while we're loading
            IsKinematic = true;
        }

        protected virtual void Update()
        {
            float dt = UnityEngine.Time.deltaTime;

            AnimationUpdate(dt);
        }

        protected virtual void FixedUpdate()
        {
            float dt = UnityEngine.Time.fixedDeltaTime;

            PhysicsUpdate(dt);
        }
#endregion

        public virtual void Initialize(ActorBehaviorData behaviorData)
        {
            _behaviorData = behaviorData;
            ResetFromData();

            IsMoving = false;

            Rotation3D = Quaternion.identity;
            Rotation2D = 0.0f;

            Velocity = Vector3.zero;

            AngularVelocity3D = Vector3.zero;
            AngularVelocity2D = 0.0f;
        }

        protected virtual void ResetFromData()
        {
            Mass = BehaviorData.Mass;
            LinearDrag = BehaviorData.Drag;
            AngularDrag = BehaviorData.AngularDrag;
            IsKinematic = BehaviorData.IsKinematic;
            UseGravity = !BehaviorData.IsKinematic;
        }

        // called by the ActorManager
        public virtual void Think(float dt)
        {
        }

        // called in Update()
        protected virtual void AnimationUpdate(float dt)
        {
        }

        // called in FixedUpdate()
        protected virtual void PhysicsUpdate(float dt)
        {
        }

#region Movement
        public virtual void Teleport(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");
            _transform.position = position;
        }

        public virtual void MoveTowards(Vector3 position, float speed, float dt)
        {
            Vector3 newPosition = Vector3.MoveTowards(Position, position, speed * dt);
            _transform.position = newPosition;
        }
#endregion

#region Events
        public virtual void OnSpawn(SpawnPoint spawnpoint)
        {
            if(null != _spawnEffect) {
                _spawnEffect.Trigger(OnSpawnComplete);
            } else {
                OnSpawnComplete();
            }
        }

        protected virtual void OnSpawnComplete()
        {
        }

        public virtual void OnReSpawn(SpawnPoint spawnpoint)
        {
            if(null != _respawnEffect) {
                _respawnEffect.Trigger(OnReSpawnComplete);
            } else {
                OnReSpawnComplete();
            }
        }

        protected virtual void OnReSpawnComplete()
        {
        }

        public virtual void OnDeSpawn()
        {
            if(null != _despawnEffect) {
                _despawnEffect.Trigger(OnDeSpawnComplete);
            } else {
                OnDeSpawnComplete();
            }
        }

        protected virtual void OnDeSpawnComplete()
        {
        }

        public virtual void CollisionEnter(GameObject collideObject)
        {
        }

        public virtual void CollisionStay(GameObject collideObject)
        {
        }

        public virtual void CollisionExit(GameObject collideObject)
        {
        }

        public virtual void TriggerEnter(GameObject triggerObject)
        {
        }

        public virtual void TriggerStay(GameObject triggerObject)
        {
        }

        public virtual void TriggerExit(GameObject triggerObject)
        {
        }
#endregion
    }
}
