using DG.Tweening;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ViewerShakeEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        [ReadOnly]
        private Viewer _viewer;

        public Viewer Viewer
        {
            get => _viewer;
            set => _viewer = value;
        }

        [SerializeField]
        private ShakeConfig _screenShakeConfig;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void OnStart()
        {
            if(EffectsManager.Instance.EnableViewerShake) {
                Viewer.Camera.DOShakePosition(_screenShakeConfig.Duration, _screenShakeConfig.Strength, _screenShakeConfig.Vibrato, _screenShakeConfig.Randomness);
            }

            _isPlaying = true;
            TimeManager.Instance.RunAfterDelay(_screenShakeConfig.Duration, () => _isPlaying = false);
        }
    }
}
