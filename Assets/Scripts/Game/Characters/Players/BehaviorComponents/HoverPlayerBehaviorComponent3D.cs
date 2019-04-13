using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Characters.BehaviorComponents;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.Players.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent3D))]
    public sealed class HoverPlayerBehaviorComponent3D : PlayerBehaviorComponent3D
    {
#region Actions
        public class HoverAction : CharacterBehaviorAction
        {
            public static HoverAction Default = new HoverAction();
        }
#endregion

        [SerializeField]
        private HoverBehaviorComponentData _data;

        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanHover => _heldSeconds >= _data.HoverHoldSeconds;

        [SerializeField]
        [ReadOnly]
        private float _hoverTimeSeconds;

        public float RemainingPercent => 1.0f - (_hoverTimeSeconds / _data.HoverTimeSeconds);

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

            if(Behavior.IsGrounded) {
                _isHeld = false;
                _heldSeconds = 0;

                StopHovering();
            }

            if(_isHeld && !IsHoverCooldown) {
                _heldSeconds += dt;
            }

            if(IsHovering) {
                _hoverTimeSeconds += dt;
                if(_hoverTimeSeconds >= _data.HoverTimeSeconds) {
                    _hoverTimeSeconds = _data.HoverTimeSeconds;
                    StopHovering();
                }
            } else if(CanHover) {
                StartHovering();
            } else if(_hoverTimeSeconds > 0.0f) {
                _hoverTimeSeconds -= dt * _data.HoverRechargeRate;
                if(_hoverTimeSeconds < 0.0f) {
                    _hoverTimeSeconds = 0.0f;
                }
            }
        }
#endregion

        public override bool OnPhysicsUpdate(float dt)
        {
            if(!IsHovering) {
                return false;
            }

            Vector3 acceleration = (_data.HoverAcceleration + Behavior.CharacterBehaviorData.FallSpeedAdjustment) * Vector3.up;
            // TODO: this probably needs to be * or / mass since it's ForceMode.Force instead of ForceMode.Acceleration
            Behavior.AddForce(acceleration, ForceMode.Force);

            return false;
        }

        public override bool OnStarted(CharacterBehaviorAction action)
        {
            if(!(action is HoverAction)) {
                return false;
            }

            if(Behavior.IsGrounded && !_data.HoverWhenGrounded) {
                return false;
            }

            _isHeld = true;
            _heldSeconds = 0;

            return true;
        }

        // NOTE: we want to consume jump actions if we're hovering
        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpBehaviorComponent3D.JumpAction)) {
                return false;
            }

            return _isHovering;
        }

        public override bool OnCancelled(CharacterBehaviorAction action)
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

            if(null != Behavior.Animator) {
                Behavior.Animator.SetBool(_data.HoverParam, true);
            }

            // stop all vertical movement immediately
            Behavior.Velocity = new Vector3(Behavior.Velocity.x, 0.0f, Behavior.Velocity.z);
        }

        public void StopHovering()
        {
            bool wasHovering = IsHovering;
            _isHovering = false;

            if(null != Behavior.Animator) {
                Behavior.Animator.SetBool(_data.HoverParam, false);
            }

            if(wasHovering) {
                _cooldownTimer.Start(_data.HoverCooldownSeconds);
            }
        }
    }
}
