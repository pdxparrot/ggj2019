using DG.Tweening;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ShakePositionEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private Transform _owner;

        [SerializeField]
        private ShakeConfig _shakeConfig;

        public override void OnStart()
        {
            _owner.transform.DOShakePosition(_shakeConfig.Duration, _shakeConfig.Strength, _shakeConfig.Vibrato, _shakeConfig.Randomness);
        }
    }
}
