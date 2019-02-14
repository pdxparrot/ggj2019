using pdxpartyparrot.Core.Input;
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

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void Initialize()
        {
// TODO: how do we get the right gamepad?
        }

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableRumble) {
                return;
            }

            if(null != _gamepadListener) {
                _gamepadListener.Rumble(_rumbleConfig);
            }

            _isPlaying = true;
            TimeManager.Instance.RunAfterDelay(_rumbleConfig.Seconds, () => _isPlaying = false);
        }
    }
}
