using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ViewerShakeEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private ShakeConfig _screenShakeConfig;

        [SerializeField]
        [ReadOnly]
        private Viewer _viewer;

        [SerializeField]
        [ReadOnly]
        private bool _isPlaying;

        public override bool IsDone => !_isPlaying;

        public override void Initialize()
        {
// TODO: how do we get the right viewer?
        }

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableViewerShake) {
                return;
            }

            Debug.LogWarning("TODO: viewer shake");

            _isPlaying = true;
            TimeManager.Instance.RunAfterDelay(_screenShakeConfig.Duration, () => _isPlaying = false);
        }
    }
}
