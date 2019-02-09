using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2019.Players;

using Spine;
using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class Environment : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation _animation;

        [SerializeField]
        private string[] _animations;

        private SpineSkinSwapper _skinSwapper;

#region Unity Lifecycle
        private void Awake()
        {
            _skinSwapper = GetComponent<SpineSkinSwapper>();

            GameManager.Instance.WaveSpawner.WaveStartEvent += WaveStartEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance && null != GameManager.Instance.WaveSpawner) {
                GameManager.Instance.WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
            }
        }
#endregion

        // TODO: move all the SetAnimation junk into a helper behavior
        private TrackEntry SetAnimation(string animationName, bool loop)
        {
            return SetAnimation(0, animationName, loop);
        }

        private TrackEntry SetAnimation(int track, string animationName, bool loop)
        {
            return _animation.AnimationState.SetAnimation(track, animationName, loop);
        }

#region Event Handlers
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            if(args.WaveIndex == 0) {
                _skinSwapper.SetSkin(0);
                SetAnimation(_animations[0], true);
            } else if(args.WaveIndex == 3) {
                _skinSwapper.SetSkin(1);
                SetAnimation(_animations[1], true);
            }  else if(args.WaveIndex == 6) {
                _skinSwapper.SetSkin(2);
                SetAnimation(_animations[2], true);
            }  else if(args.WaveIndex == 9) {
                _skinSwapper.SetSkin(3);
                SetAnimation(_animations[0], true);
            }
        }
#endregion
    }
}
