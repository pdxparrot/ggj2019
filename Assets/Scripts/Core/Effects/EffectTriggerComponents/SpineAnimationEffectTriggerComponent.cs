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
                _trackEntry.Complete += OnComplete;
            } else {
                // TODO: set a timer or something to timeout when we'd normally be done
            }
        }

        public override void OnStop()
        {
            // TODO: any way to force-stop the animation?

            if(null != _trackEntry) {
                _trackEntry.Complete -= OnComplete;
                _trackEntry = null;
            }
        }

#region Event Handlers
        private void OnComplete(TrackEntry entry)
        {
            entry.Complete -= OnComplete;
            if(entry == _trackEntry) {
                _trackEntry = null;
            }
        }
#endregion
    }
}
#endif
