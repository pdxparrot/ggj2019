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

        public override void Initialize()
        {
// TODO: how do we get the right gamepad?
        }

        public override void OnStart()
        {
            if(null != _gamepadListener) {
                _gamepadListener.Rumble(_rumbleConfig);
            }
        }
    }
}
