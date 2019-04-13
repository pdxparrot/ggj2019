using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    [RequireComponent(typeof(JumpBehaviorComponent3D))]
    public sealed class LongJumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        [SerializeField]
        private LongJumpBehaviorComponentData _data;

        [Space(10)]

#region Effects
        [Header("Effects")]

        [SerializeField]
        [CanBeNull]
        private EffectTrigger _longJumpEffect;
#endregion

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
                    Behavior.Jump(_data.LongJumpHeight);
                    if(null != _longJumpEffect) {
                        _longJumpEffect.Trigger();
                    }

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
