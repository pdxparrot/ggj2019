using JetBrains.Annotations;

using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Actors
{
    public sealed class GamepadListener : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _gamepadId;

        [SerializeField]
        [ReadOnly]
        private bool _isRumbling;

        [CanBeNull]
        private Gamepad _gamepad;

        [CanBeNull]
        public Gamepad Gamepad => _gamepad;

        public bool HasGamepad => null != _gamepad;

#region Unity Lifecycle
        private void Awake()
        {
            _gamepadId = InputManager.Instance.AcquireGamepad(OnAcquireGamepad, OnGamepadDisconnect);
        }

        private void OnDestroy()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.ReleaseGamepad(_gamepadId);
            }

            _gamepadId = 0;
            _gamepad = null;
        }
#endregion

        public bool IsOurGamepad(InputAction.CallbackContext context)
        {
            return context.control.device == _gamepad;
        }

        public void Rumble(float seconds, float lowFrequency, float highFrequence)
        {
            if(!HasGamepad || _isRumbling) {
                return;
            }

            Gamepad.SetMotorSpeeds(lowFrequency, highFrequence);
            _isRumbling = true;

            TimeManager.Instance.RunAfterDelay(seconds, () => {
                if(HasGamepad) {
                    Gamepad.SetMotorSpeeds(0.0f, 0.0f);
                }
                _isRumbling = false;
            });
        }

#region Event Handlers
        private void OnAcquireGamepad(Gamepad gamepad)
        {
            _gamepad = gamepad;
        }

        private void OnGamepadDisconnect(Gamepad gamepad)
        {
            _gamepad = null;
        }
#endregion
    }
}
