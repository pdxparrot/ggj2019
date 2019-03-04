#if USE_SPINE
using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public sealed class SpineSkinHelper : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation _skeletonAnimation;

        [SerializeField]
        private string[] _skinNames;

        public Color Color
        {
            get => _skeletonAnimation.Skeleton.GetColor();
            set => _skeletonAnimation.Skeleton.SetColor(value);
        }

        public float Red
        {
            get => _skeletonAnimation.Skeleton.R;
            set => _skeletonAnimation.Skeleton.R = value;
        }

        public float Green
        {
            get => _skeletonAnimation.Skeleton.G;
            set => _skeletonAnimation.Skeleton.G = value;
        }

        public float Blue
        {
            get => _skeletonAnimation.Skeleton.B;
            set => _skeletonAnimation.Skeleton.B = value;
        }

        public float Alpha
        {
            get => _skeletonAnimation.Skeleton.A;
            set => _skeletonAnimation.Skeleton.A = value;
        }

        public string Skin
        {
            get => _skeletonAnimation.Skeleton.Skin.Name;
            set {
                _skeletonAnimation.Skeleton.SetSkin(value);
                _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
                _skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
            }
        }

        public void SetSkin(int index)
        {
            if(index < 0 || index >= _skinNames.Length) {
                Debug.LogWarning($"Attempted to set invalid skin {index} ({_skinNames.Length} total)");
                return;
            }
            Skin = _skinNames[index];
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
