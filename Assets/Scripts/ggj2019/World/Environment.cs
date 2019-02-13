using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    [RequireComponent(typeof(SpineSkinSwapper))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public class Environment : MonoBehaviour
    {
        [SerializeField]
        protected string[] _animations;

        private SpineSkinSwapper _skinSwapper;

        private SpineAnimationHelper _spineAnimationHelper;

#region Unity Lifecycle
        private void Awake()
        {
            _skinSwapper = GetComponent<SpineSkinSwapper>();
            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();

            GameManager.Instance.WaveSpawner.WaveStartEvent += WaveStartEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance && null != GameManager.Instance.WaveSpawner) {
                GameManager.Instance.WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
            }
        }
#endregion

#region Event Handlers
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            TrackEntry trackEntry = null;
            if(args.WaveIndex == 0) {
                _skinSwapper.SetSkin(0);
                trackEntry = _spineAnimationHelper.SetAnimation(_animations[0], true);
            } else if(args.WaveIndex == 3) {
                _skinSwapper.SetSkin(1);
                trackEntry = _spineAnimationHelper.SetAnimation(_animations[1], true);
            }  else if(args.WaveIndex == 6) {
                _skinSwapper.SetSkin(2);
                trackEntry = _spineAnimationHelper.SetAnimation(_animations[2], true);
            }  else if(args.WaveIndex == 9) {
                _skinSwapper.SetSkin(3);
                trackEntry = _spineAnimationHelper.SetAnimation(_animations[0], true);
            }

            if(null != trackEntry) {
                OnWaveAnimationSet(args.WaveIndex, trackEntry);
            }
        }
#endregion

        protected virtual void OnWaveAnimationSet(int waveIndex, TrackEntry trackEntry)
        {
        }
    }
}
