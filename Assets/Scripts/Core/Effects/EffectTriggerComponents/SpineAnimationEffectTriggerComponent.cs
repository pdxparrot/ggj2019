#if USE_SPINE
using pdxpartyparrot.Core.Animation;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class SpineAnimationEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private SpineAnimationHelper _spineAnimation;

        [SerializeField]
        private string _spineAnimationName = "default";

        [SerializeField]
        private int _spineAnimationTrack;

        public override void OnStart()
        {
            _spineAnimation.SetAnimation(_spineAnimationTrack, _spineAnimationName, false);
        }
    }
}
#endif
