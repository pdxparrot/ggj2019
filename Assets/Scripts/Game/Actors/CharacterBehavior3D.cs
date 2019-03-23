using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors.BehaviorComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Actors
{
    public class CharacterBehavior3D : ActorBehavior3D
    {
        public CharacterBehaviorData CharacterBehaviorData => (CharacterBehaviorData)BehaviorData;

        [Space(10)]

#region Physics
        [Header("Character Physics")]

        [SerializeField]
        [ReadOnly]
        private bool _isGrounded;

        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        [SerializeField]
        [ReadOnly]
        private bool _isSliding;

        public bool IsSliding
        {
            get => _isSliding;
            set => _isSliding = value;
        }

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

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

// TODO: reorderable list (don't use GetComponents())
        private CharacterBehaviorComponent3D[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _components = GetComponents<CharacterBehaviorComponent3D>();
            //Debug.Log($"Found {_components.Length} CharacterBehaviorComponent3Ds");
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
            UseGravity = !IsKinematic && (!IsGrounded || IsMoving || IsSliding);
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
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is CharacterBehaviorData);

            base.Initialize(behaviorData);
        }

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

        public override void AnimationMove(Vector2 direction, float dt)
        {
            if(!CanMove) {
                return;
            }

            if(RunOnComponents(c => c.OnAnimationMove(direction, dt))) {
                return;
            }

            DefaultAnimationMove(direction, dt);
        }

        public virtual void DefaultAnimationMove(Vector2 direction, float dt)
        {
            // align with the movement
            Vector3 forward = new Vector3(direction.x, 0.0f, direction.y);
            if(forward.sqrMagnitude > float.Epsilon) {
                Owner.transform.forward = forward;
            }

            if(null != Animator) {
                Animator.SetFloat(CharacterBehaviorData.MoveXAxisParam, CanMove ? Mathf.Abs(direction.x) : 0.0f);
                Animator.SetFloat(CharacterBehaviorData.MoveZAxisParam, CanMove ? Mathf.Abs(direction.y) : 0.0f);
            }
        }

        public override void PhysicsMove(Vector2 direction, float dt)
        {
            if(!CanMove) {
                return;
            }

            float speed = CharacterBehaviorData.MoveSpeed;

            if(RunOnComponents(c => c.OnPhysicsMove(direction, speed, dt))) {
                return;
            }

            if(!CharacterBehaviorData.AllowAirControl && IsFalling) {
                return;
            }

            DefaultPhysicsMove(direction, speed, dt);
        }

        public virtual void DefaultPhysicsMove(Vector2 direction, float speed, float dt)
        {
            Vector3 fixedDirection = new Vector3(direction.x, 0.0f, direction.y);

            Vector3 velocity = fixedDirection * speed;
            Quaternion rotation = Owner.Behavior.Rotation3D;
            velocity = rotation * velocity;

            if(IsKinematic) {
                MovePosition(Position + velocity * dt);
            } else {
                velocity.y = Velocity.y;
                Velocity = velocity;
            }
        }

        public virtual void Jump(float height)
        {
            if(!CanMove) {
                return;
            }

            // force physics to a sane state for the first frame of the jump
            UseGravity = true;
            IsGrounded = false;

            // factor in fall speed adjust
            float gravity = -Physics.gravity.y + CharacterBehaviorData.FallSpeedAdjustment;

            // v = sqrt(2gh)
            Velocity = Vector3.up * Mathf.Sqrt(height * 2.0f * gravity);
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
    }
}
