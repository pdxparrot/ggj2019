using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent3D))]
    public sealed class LongJumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        [SerializeField]
        private LongJumpBehaviorComponentData _data;

        [SerializeField]
        [ReadOnly]
        private bool _isHeld;

        [SerializeField]
        [ReadOnly]
        private float _heldSeconds;

        private bool CanLongJump => !_didLongJump && _heldSeconds >= _data.LongJumpHoldSeconds;

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
                    Behavior.Jump(_data.LongJumpHeight, _data.LongJumpParam);
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
