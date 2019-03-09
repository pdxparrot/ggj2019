using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.BehaviorComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterBehavior3D : ActorBehavior3D, ICharacterBehavior
    {
        public CharacterBehaviorData CharacterBehaviorData => (CharacterBehaviorData)BehaviorData;

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

        private float GroundCheckRadius => Owner.Height - 0.1f;

        protected Vector3 GroundCheckCenter => Owner.transform.position + (GroundCheckRadius * Vector3.up);
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

#region Physics
        public override bool UseGravity
        {
            get => base.UseGravity;
            set
            {
                base.UseGravity = value;
                if(!value) {
                    Velocity = Vector3.zero;
                }
            }
        }

        public bool IsFalling => UseGravity && (!IsGrounded && !IsSliding && Velocity.y < 0.0f);
#endregion

        public CapsuleCollider Capsule => (CapsuleCollider)Owner3D.Collider;

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

        private CharacterBehaviorComponent3D[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(BehaviorData is CharacterBehaviorData);

            _components = GetComponents<CharacterBehaviorComponent3D>();
            //Debug.Log($"Found {_components.Length} CharacterBehaviorComponent3Ds");

            if(!BehaviorData.IsKinematic) {
                StartCoroutine(RaycastRoutine());
            }
        }

        protected override void Update()
        {
            base.Update();

            if(null != Animator) {
                Animator.SetBool(CharacterBehaviorData.FallingParam, IsFalling);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            UseGravity = !IsGrounded || IsMoving || IsSliding;
            IsKinematic = IsKinematic;
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

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (CharacterBehaviorData.GroundedEpsilon * Vector3.down), GroundCheckRadius);

            Gizmos.color = DidGroundCheckCollide ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (CharacterBehaviorData.GroundCheckLength * Vector3.down), GroundCheckRadius);
        }
#endregion

        protected override void InitRigidbody(Rigidbody rb)
        {
            base.InitRigidbody(rb);

            rb.isKinematic = BehaviorData.IsKinematic;
            rb.useGravity = !BehaviorData.IsKinematic;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.detectCollisions = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation.None;
        }

#region Components
        [CanBeNull]
        public T GetBehaviorComponent<T>() where T: CharacterBehaviorComponent
        {
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    return tc;
                }
            }
            return null;
        }

        public void GetBehaviorComponents<T>(ICollection<T> components) where T: CharacterBehaviorComponent
        {
            components.Clear();
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    components.Add(tc);
                }
            }
        }

        public bool RunOnComponents(Func<CharacterBehaviorComponent, bool> f)
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
        public virtual void ActionStarted(CharacterBehaviorComponent.CharacterBehaviorAction action)
        {
            RunOnComponents(c => c.OnStarted(action));
        }

        public virtual void ActionPerformed(CharacterBehaviorComponent.CharacterBehaviorAction action)
        {
            RunOnComponents(c => c.OnPerformed(action));
        }

        public virtual void ActionCancelled(CharacterBehaviorComponent.CharacterBehaviorAction action)
        {
            RunOnComponents(c => c.OnCancelled(action));
        }
#endregion

        public override void AnimationMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(RunOnComponents(c => c.OnAnimationMove(axes, dt))) {
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
                Owner.transform.forward = forward;
            }

            if(null != Animator) {
                Animator.SetFloat(CharacterBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
                Animator.SetFloat(CharacterBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
            }
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            float speed = CharacterBehaviorData.MoveSpeed;

            if(RunOnComponents(c => c.OnPhysicsMove(axes, speed, dt))) {
                return;
            }

            if(!CharacterBehaviorData.AllowAirControl && IsFalling) {
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
            if(GroundSlope >= CharacterBehaviorData.SlopeLimit) {
                float dp = Vector3.Dot(Owner.transform.forward, GroundCheckNormal);
                if(dp < 0.0f) {
                    fixedAxes.z = 0.0f;
                }
            }

            Vector3 velocity = fixedAxes * speed;
            Quaternion rotation = null != Owner.Viewer ? Quaternion.AngleAxis(Owner.Viewer.transform.localEulerAngles.y, Vector3.up) : Owner.transform.rotation;
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
            UseGravity = true;
            ResetGroundCheck();

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + CharacterBehaviorData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);

            // TODO: move to an EffectTrigger
            if(null != Animator) {
                Animator.SetTrigger(animationParam);
            }
        }

        public void ResetGroundCheck()
        {
            _didGroundCheckCollide = _isGrounded = false;
        }

        private void FudgeVelocity(float dt)
        {
            Vector3 adjustedVelocity = Velocity;
            if(IsGrounded && !IsMoving) {
                // prevent any weird ground adjustment shenanigans
                // when we're grounded and not moving
                adjustedVelocity.y = 0.0f;
            } else if(UseGravity) {
                // do some fudging to jumping/falling so it feels better
                float adjustment = CharacterBehaviorData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -CharacterBehaviorData.TerminalVelocity) {
                    adjustedVelocity.y = -CharacterBehaviorData.TerminalVelocity;
                }
            }
            Velocity = adjustedVelocity;
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

            int hitCount = Physics.SphereCastNonAlloc(origin, GroundCheckRadius, Vector3.down, _groundCheckHits, CharacterBehaviorData.GroundCheckLength, CharacterBehaviorData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore);
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
            Profiler.BeginSample("CharacterBehavior3D.UpdateIsGrounded");
            try {
                bool wasGrounded = IsGrounded;

                _didGroundCheckCollide = CheckIsGrounded(out _groundCheckMinDistance);

                if(IsKinematic) {
                    // something else is handling this case?
                } else {
                    _isGrounded = _didGroundCheckCollide && _groundCheckMinDistance < CharacterBehaviorData.GroundedEpsilon;
                }

                // if we're on a slope, we're sliding down it
                _isSliding = _groundSlope >= CharacterBehaviorData.SlopeLimit;

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
