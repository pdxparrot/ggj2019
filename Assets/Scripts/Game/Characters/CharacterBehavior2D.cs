using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Characters.BehaviorComponents;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters
{
    public abstract class CharacterBehavior2D : ActorBehavior2D
    {
        public CharacterMovement2D CharacterMovement2D => (CharacterMovement2D)Movement2D;

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

        public bool IsFalling => CharacterMovement2D.UseGravity && (!IsGrounded && !IsSliding && CharacterMovement2D.Velocity.y < 0.0f);
#endregion

        public override bool CanMove => base.CanMove && !GameStateManager.Instance.GameManager.IsGameOver;

// TODO: reorderable list (don't use GetComponents())
        private CharacterBehaviorComponent2D[] _components;

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Movement2D is CharacterMovement2D);

            base.Awake();

            _components = GetComponents<CharacterBehaviorComponent2D>();
            //Debug.Log($"Found {_components.Length} CharacterBehaviorComponent2Ds");
        }

        protected override void Update()
        {
            base.Update();

#if !USE_SPINE
            if(null != Animator) {
                Animator.SetBool(CharacterBehaviorData.FallingParam, IsFalling);
            }
#endif
        }
#endregion

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is CharacterBehaviorData);

            base.Initialize(behaviorData);
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

        public void RunOnComponents(Func<CharacterBehaviorComponent, bool> f)
        {
            foreach(CharacterBehaviorComponent2D component in _components) {
                if(f(component)) {
                    return;
                }
            }
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

        protected override void AnimationUpdate(float dt)
        {
            if(!CanMove) {
                return;
            }

            RunOnComponents(c => c.OnAnimationUpdate(dt));
        }

        protected override void PhysicsUpdate(float dt)
        {
            if(!CanMove) {
                return;
            }

            RunOnComponents(c => c.OnPhysicsUpdate(dt));
        }
    }
}