#if USE_SPINE
using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public sealed class SpineSkinSwapper : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation _skeletonAnimation;

        [SerializeField]
        private string[] _skinNames;

        public void SetSkin(int index)
        {
            if(index < 0 || index >= _skinNames.Length) {
                Debug.LogWarning($"Attempted to set invalid skin {index} ({_skinNames.Length} total)");
                return;
            }

            string skinName = _skinNames[index];
            _skeletonAnimation.Skeleton.SetSkin(skinName);
            _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
        }

        public void SetRandomSkin()
        {
            if(_skinNames.Length < 1) {
                return;
            }
            SetSkin(PartyParrotManager.Instance.Random.Next(_skinNames.Length));
        }
    }
}
#endif
