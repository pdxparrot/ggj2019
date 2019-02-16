#if USE_SPINE
using pdxpartyparrot.Core.Animation;

using Spine;

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

        private TrackEntry _trackEntry;

        public override bool IsDone => null == _trackEntry || _trackEntry.IsComplete;

        public override void OnStart()
        {
            if(EffectsManager.Instance.EnableAnimation) {
                _trackEntry = _spineAnimation.SetAnimation(_spineAnimationTrack, _spineAnimationName, false);
                _trackEntry.Complete += te => {
                    _trackEntry = null;
                };
            } else {
                // TODO: set a timer or something to timeout when we'd normally be done
            }
        }
    }
}
#endif
