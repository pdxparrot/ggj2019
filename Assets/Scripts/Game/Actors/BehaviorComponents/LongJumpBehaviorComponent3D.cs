using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent3D))]
    public sealed class LongJumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanLongJump => !_didLongJump && _heldSeconds >= Behavior.BehaviorData.LongJumpHoldSeconds;

        [SerializeField]
        [ReadOnly]
        private bool _didLongJump;

#region Unity Lifecycle
        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            if(!Behavior.IsGrounded) {
                _isHeld = false;
                _heldSeconds = 0;
            }

            if(_isHeld) {
                _heldSeconds += dt;

                if(CanLongJump) {
                    Behavior.Jump(Behavior.BehaviorData.LongJumpHeight, Behavior.BehaviorData.LongJumpParam);
                    _didLongJump = true;
                }
            }
        }
#endregion

        public override bool OnStarted(CharacterBehaviorAction action)
        {
            if(!(action is JumpBehaviorComponent3D.JumpAction)) {
                return false;
            }

            if(!Behavior.IsGrounded || Behavior.IsSliding) {
                return false;
            }

            _isHeld = true;
            _heldSeconds = 0;
            _didLongJump = false;

            return true;
        }

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpBehaviorComponent3D.JumpAction)) {
                return false;
            }

            _isHeld = false;
            _heldSeconds = 0;

            return _didLongJump;
        }
    }
}
