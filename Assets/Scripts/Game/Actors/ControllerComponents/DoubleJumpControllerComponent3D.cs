﻿using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    [RequireComponent(typeof(JumpControllerComponent3D))]
    public sealed class DoubleJumpControllerComponent3D : CharacterActorControllerComponent3D
    {
        [SerializeField]
        [ReadOnly]
        private int _doubleJumpCount;

        private bool CanDoubleJump => !Behavior.IsGrounded && (Behavior.ControllerData.DoubleJumpCount < 0 || _doubleJumpCount < Behavior.ControllerData.DoubleJumpCount);

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

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpControllerComponent3D.JumpAction)) {
                return false;
            }

            if(!CanDoubleJump) {
                return false;
            }

            Behavior.Jump(Behavior.ControllerData.DoubleJumpHeight, Behavior.ControllerData.DoubleJumpParam);

            _doubleJumpCount++;
            return true;
        }
    }
}
