﻿using DG.Tweening;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ShakePositionEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private Transform _owner;

        [SerializeField]
        private ShakeConfig _shakeConfig;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableShakePosition) {
                return;
            }

            _owner.transform.DOShakePosition(_shakeConfig.Duration, _shakeConfig.Strength, _shakeConfig.Vibrato, _shakeConfig.Randomness);

            _isPlaying = true;
            TimeManager.Instance.RunAfterDelay(_shakeConfig.Duration, () => _isPlaying = false);
        }
    }
}