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

        public override void Initialize()
        {
// TODO: how do we get the right viewer?
        }

        public override void OnStart()
        {
            Debug.LogWarning("TODO: viewer shake");
        }
    }
}
