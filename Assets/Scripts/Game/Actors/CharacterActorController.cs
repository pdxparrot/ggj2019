using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
    public interface ICharacterActorController
    {
        CharacterActorControllerData ControllerData { get; }

#if !USE_SPINE
        Animator Animator { get; }
#endif

        ActorAnimator ActorAnimator { get; }

        // TODO: this could move to the data
        float RaycastRoutineRate { get; }

        Actor Owner { get; }

        Vector3 Position { get; set; }

        Quaternion Rotation3D { get; set; }

        float Rotation2D { get; set; }

        Vector3 Velocity { get; set; }

        bool CanMove { get; }

        bool IsMoving { get; }

        bool IsGrounded { get; }

        bool DidGroundCheckCollide { get; }

        bool IsSliding { get; }

        bool UseGravity { get; set; }

        Vector3 LastMoveAxes { get; }

        void MovePosition(Vector3 position);

        void AddForce(Vector3 force);

        void DefaultAnimationMove(Vector3 axes, float dt);

        void DefaultPhysicsMove(Vector3 axes, float speed, float dt);

        void Jump(float height, string animationParam);
    }

    // TODO: what's left of this can be broken down into components
    // that can be added to the 2D/3D components
    // (this thing is vestigial from the pre-dimension split era)
    public class CharacterActorController : MonoBehaviour
    {
        [SerializeField]
        private CharacterActorControllerData _controllerData;

        public CharacterActorControllerData ControllerData => _controllerData;

        [SerializeField]
        private Actor _owner;

        public Actor Owner => _owner;

        [Space(10)]

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("How often to run raycast checks, in seconds")]
        private float _raycastRoutineRate = 0.1f;

        public float RaycastRoutineRate => _raycastRoutineRate;

        [Space(10)]

#region Ground Check
        [Header("Ground Check")]

        private RaycastHit[] _groundCheckHits = new RaycastHit[4];

        [SerializeField]
        [ReadOnly]
        private bool _didGroundCheckCollide;

        public bool DidGroundCheckCollide => _didGroundCheckCollide;

        [SerializeField]
        [ReadOnly]
        private Vector3 _groundCheckNormal;

        public Vector3 GroundCheckNormal => _groundCheckNormal;

        [SerializeField]
        [ReadOnly]
        private float _groundCheckMinDistance;

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded
        {
            get => _isGrounded;
            protected set => _isGrounded = value;
        }

        private float GroundCheckRadius => _owner.Height - 0.1f;

        protected Vector3 GroundCheckCenter => transform.position + (GroundCheckRadius * Vector3.up);
#endregion

        [Space(10)]

#region Slope Check
        [Header("Slope Check")]

        [SerializeField]
        [ReadOnly]
        private float _groundSlope;

        public float GroundSlope => _groundSlope;

        [SerializeField]
        [ReadOnly]
        private bool _isSliding;

        public bool IsSliding => _isSliding;
#endregion

        [Space(10)]

#region Effects
        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _groundedEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _isKinematic;

        public bool IsKinematic
        {
            get => _isKinematic;
            set => _isKinematic = value;
        }

        private CharacterActorControllerComponent[] _components;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            _components = GetComponents<CharacterActorControllerComponent>();
            //Debug.Log($"Found {_components.Length} CharacterActorControllerComponents");

            if(!GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic) {
                StartCoroutine(RaycastRoutine());
            }
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundedEpsilon * Vector3.down), GroundCheckRadius);

            Gizmos.color = DidGroundCheckCollide ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundCheckLength * Vector3.down), GroundCheckRadius);
        }
#endregion

#region Components
        [CanBeNull]
        public T GetControllerComponent<T>() where T: CharacterActorControllerComponent
        {
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    return tc;
                }
            }
            return null;
        }

        public void GetControllerComponents<T>(ICollection<T> components) where T: CharacterActorControllerComponent
        {
            components.Clear();
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    components.Add(tc);
                }
            }
        }

        public bool RunOnComponents(Func<CharacterActorControllerComponent, bool> f)
        {
            foreach(var component in _components) {
                if(f(component)) {
                    return true;
                }
            }
            return false;
        }
#endregion

#region Actions
        public virtual void ActionStarted(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnStarted(action));
        }

        public virtual void ActionPerformed(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnPerformed(action));
        }

        public virtual void ActionCancelled(CharacterActorControllerComponent.CharacterActorControllerAction action)
        {
            RunOnComponents(c => c.OnCancelled(action));
        }
#endregion

        public void ResetGroundCheck()
        {
            _didGroundCheckCollide = _isGrounded = false;
        }

        private IEnumerator RaycastRoutine()
        {
            Debug.Log("Starting character raycast routine");

            WaitForSeconds wait = new WaitForSeconds(RaycastRoutineRate);
            while(true) {
                UpdateIsGrounded();

                yield return wait;
            }
        }

#region Grounded Check
        protected bool CheckIsGrounded(out float minDistance)
        {
            minDistance = float.MaxValue;

            Vector3 origin = GroundCheckCenter;

            int hitCount = Physics.SphereCastNonAlloc(origin, GroundCheckRadius, Vector3.down, _groundCheckHits, ControllerData.GroundCheckLength, ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore);
            if(hitCount < 1) {
                // no slope if not grounded
                _groundSlope = 0;
                return false;
            }

            // figure out the slope of whatever we hit
            _groundCheckNormal = Vector3.zero;
            for(int i=0; i<hitCount; ++i) {
                _groundCheckNormal += _groundCheckHits[i].normal;
                minDistance = Mathf.Min(minDistance, _groundCheckHits[i].distance);
            }
            _groundCheckNormal /= hitCount;

            _groundSlope = Vector3.Angle(Vector3.up, _groundCheckNormal);

            return true;
        }

        private void UpdateIsGrounded()
        {
            Profiler.BeginSample("Character.UpdateIsGrounded");
            try {
                bool wasGrounded = IsGrounded;

                _didGroundCheckCollide = CheckIsGrounded(out _groundCheckMinDistance);

                if(IsKinematic) {
                    // something else is handling this case?
                } else {
                    _isGrounded = _didGroundCheckCollide && _groundCheckMinDistance < ControllerData.GroundedEpsilon;
                }

                // if we're on a slope, we're sliding down it
                _isSliding = _groundSlope >= ControllerData.SlopeLimit;

                if(!wasGrounded && IsGrounded) {
                    if(null != _groundedEffect) {
                        _groundedEffect.Trigger();
                    }
                }
            } finally {
                Profiler.EndSample();
            }
        }
#endregion
    }
}
