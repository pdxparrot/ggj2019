using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors
{
    // TODO: merge this into ActorBehavior2D
    [RequireComponent(typeof(Collider2D))]
    public class CharacterBehavior2D : ActorBehavior2D
    {
        [SerializeField]
        private CharacterActorControllerData _controllerData;

        public CharacterActorControllerData ControllerData => _controllerData;

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

        [Space(10)]

#region Animation
        [Header("Animation")]

#if USE_SPINE
        [SerializeField]
        private SpineAnimationHelper _spineAnimation;

        protected SpineAnimationHelper SpineAnimation => _spineAnimation;
#endif
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [ReadOnly]
        private bool _useGravity = true;

        public virtual bool UseGravity
        {
            get => _useGravity;
            set
            {
                _useGravity = value;
                Velocity = Vector3.zero;
            }
        }

        public bool IsFalling => UseGravity && (!IsGrounded && !IsSliding && Velocity.y < 0.0f);
#endregion

        public Collider2D Collider => Owner2D.Collider;

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

        private CharacterActorControllerComponent2D[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _components = GetComponents<CharacterActorControllerComponent2D>();
            //Debug.Log($"Found {_components.Length} CharacterActorControllerComponents");

            if(!GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic) {
                StartCoroutine(RaycastRoutine());
            }
        }

        protected override void Update()
        {
            base.Update();

#if !USE_SPINE
            if(null != Animator) {
                Animator.SetBool(ControllerData.FallingParam, IsFalling);
            }
#endif
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float dt = Time.fixedDeltaTime;

            FudgeVelocity(dt);

            // turn off gravity if we're grounded and not moving and not sliding
            // this should stop us sliding down slopes we shouldn't slide down
            SetUseGravity(UseGravity && (!IsGrounded || IsMoving || IsSliding));
            IsKinematic = IsKinematic;
        }

        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

            /*Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Position + AngularVelocity2D);*/

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Position, Position + Velocity);

            Gizmos.color = IsGrounded ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundedEpsilon * Vector3.down), GroundCheckRadius);

            Gizmos.color = DidGroundCheckCollide ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(GroundCheckCenter + (ControllerData.GroundCheckLength * Vector3.down), GroundCheckRadius);
        }
#endregion

        protected override void InitRigidbody(Rigidbody2D rb)
        {
            base.InitRigidbody(rb);

            rb.isKinematic = GameStateManager.Instance.PlayerManager.PlayerData.IsKinematic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation2D.None;
        }

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

#if USE_SPINE
            SpineAnimation.SetFacing(LastMoveAxes);
#else
            // TODO: set facing (set localScale.x)
            if(null != Animator) {
                Animator.SetFloat(ControllerData.MoveXAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.x) : 0.0f);
                Animator.SetFloat(ControllerData.MoveZAxisParam, CanMove ? Mathf.Abs(LastMoveAxes.y) : 0.0f);
            }
#endif
        }

        public override void PhysicsMove(Vector3 axes, float dt)
        {
            if(!CanMove) {
                return;
            }

            float speed = ControllerData.MoveSpeed;

            if(RunOnComponents(c => c.OnPhysicsMove(axes, speed, dt))) {
                return;
            }

            if(!ControllerData.AllowAirControl && IsFalling) {
                return;
            }

            DefaultPhysicsMove(axes, speed, dt);
        }

        public virtual void DefaultPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(!CanMove) {
                return;
            }

            // TODO: handle slopes

            Vector3 velocity = axes * speed;
            if(!IsKinematic) {
                velocity.y = Velocity.y;
            }

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
            _useGravity = true;
            ResetGroundCheck();

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + ControllerData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);

            // TODO: move to an EffectTrigger
#if !USE_SPINE
            if(null != Animator) {
                Animator.SetTrigger(animationParam);
            }
#endif
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
                float adjustment = ControllerData.FallSpeedAdjustment * dt;
                adjustedVelocity.y -= adjustment;

                // apply terminal velocity
                if(adjustedVelocity.y < -ControllerData.TerminalVelocity) {
                    adjustedVelocity.y = -ControllerData.TerminalVelocity;
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
            Profiler.BeginSample("CharacterBehavior2D.UpdateIsGrounded");
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