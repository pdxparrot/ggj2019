using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class RumbleEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private RumbleConfig _rumbleConfig;

        [SerializeField]
        [ReadOnly]
        private GamepadListener _gamepadListener;

        public GamepadListener GamepadListener
        {
            get => _gamepadListener;
            set => _gamepadListener = value;
        }

        [SerializeField]
        private bool _waitForComplete;

        public override bool WaitForComplete => _waitForComplete;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void OnStart()
        {
            if(EffectsManager.Instance.EnableRumble) {
                _gamepadListener.Rumble(_rumbleConfig);
            }

            _isPlaying = true;
            TimeManager.Instance.RunAfterDelay(_rumbleConfig.Seconds, () => _isPlaying = false);
        }
    }
}
