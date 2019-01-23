﻿using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    [RequireComponent(typeof(JumpControllerComponent))]
    public sealed class HoverControllerComponent : CharacterActorControllerComponent
    {
#region Actions
        public class HoverAction : CharacterActorControllerAction
        {
            public static HoverAction Default = new HoverAction();
        }
#endregion

        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanHover => _heldSeconds >= Controller.ControllerData.HoverHoldSeconds;

        [SerializeField]
        [ReadOnly]
        private float _hoverTimeSeconds;

        public float RemainingPercent => 1.0f - (_hoverTimeSeconds / Controller.ControllerData.HoverTimeSeconds);

        [SerializeField]
        [ReadOnly]
        private Timer _cooldownTimer;

        private bool IsHoverCooldown => _cooldownTimer.SecondsRemaining > 0.0f;

        [SerializeField]
        [ReadOnly]
        private bool _isHovering;

        public bool IsHovering => _isHovering;

#region Unity Lifecycle
        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            _cooldownTimer.Update(dt);

            if(Controller.IsGrounded) {
                _isHeld = false;
                _heldSeconds = 0;

                StopHovering();
            }

            if(_isHeld && !IsHoverCooldown) {
                _heldSeconds += dt;
            }

            if(IsHovering) {
                _hoverTimeSeconds += dt;
                if(_hoverTimeSeconds >= Controller.ControllerData.HoverTimeSeconds) {
                    _hoverTimeSeconds = Controller.ControllerData.HoverTimeSeconds;
                    StopHovering();
                }
            } else if(CanHover) {
                StartHovering();
            } else if(_hoverTimeSeconds > 0.0f) {
                _hoverTimeSeconds -= dt * Controller.ControllerData.HoverRechargeRate;
                if(_hoverTimeSeconds < 0.0f) {
                    _hoverTimeSeconds = 0.0f;
                }
            }
        }
#endregion

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsHovering) {
                return false;
            }

            Vector3 acceleration = (Controller.ControllerData.HoverAcceleration + Controller.ControllerData.FallSpeedAdjustment) * Vector3.up;
            // TODO: this probably needs to be * or / mass since it's ForceMode.Force instead of ForceMode.Acceleration
            Controller.AddForce(acceleration);

            Controller.DefaultPhysicsMove(axes, Controller.ControllerData.HoverMoveSpeed, dt);

            return true;
        }

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is HoverAction)) {
                return false;
            }

            if(Controller.IsGrounded && !Controller.ControllerData.HoverWhenGrounded) {
                return false;
            }

            _isHeld = true;
            _heldSeconds = 0;

            return true;
        }

        // NOTE: we want to consume jump actions if we're hovering
        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpControllerComponent.JumpAction)) {
                return false;
            }

            return _isHovering;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is HoverAction)) {
                return false;
            }

            if(!IsHovering) {
                return false;
            }

            _isHeld = false;
            _heldSeconds = 0;

            bool wasHover = _isHovering;
            StopHovering();

            return wasHover;
        }

        private void StartHovering()
        {
            _isHovering = true;

            if(null != Controller.Owner.Animator) {
                Controller.Owner.Animator.SetBool(Controller.ControllerData.HoverParam, true);
            }

            // stop all vertical movement immediately
            Controller.Velocity = new Vector3(Controller.Velocity.x, 0.0f, Controller.Velocity.z);
        }

        public void StopHovering()
        {
            bool wasHovering = IsHovering;
            _isHovering = false;

            if(null != Controller.Owner.Animator) {
                Controller.Owner.Animator.SetBool(Controller.ControllerData.HoverParam, false);
            }

            if(wasHovering) {
                _cooldownTimer.Start(Controller.ControllerData.HoverCooldownSeconds);
            }
        }
    }
}
