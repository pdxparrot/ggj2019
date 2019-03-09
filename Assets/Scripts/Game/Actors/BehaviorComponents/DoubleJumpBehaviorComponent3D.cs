using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent3D))]
    public sealed class DoubleJumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        [SerializeField]
        [ReadOnly]
        private int _doubleJumpCount;

        private bool CanDoubleJump => !Behavior.IsGrounded && (Behavior.BehaviorData.DoubleJumpCount < 0 || _doubleJumpCount < Behavior.BehaviorData.DoubleJumpCount);

#region Unity Lifecycle
        private void Update()
        {
            if(Behavior.IsGrounded) {
                _doubleJumpCount = 0;
            }
        }
#endregion

        public void Reset()
        {
            _doubleJumpCount = 0;
        }

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpBehaviorComponent3D.JumpAction)) {
                return false;
            }

            if(!CanDoubleJump) {
                return false;
            }

            Behavior.Jump(Behavior.BehaviorData.DoubleJumpHeight, Behavior.BehaviorData.DoubleJumpParam);

            _doubleJumpCount++;
            return true;
        }
    }
}
