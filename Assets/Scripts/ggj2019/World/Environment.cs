using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2019.Players;

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
        private AudioClip _wave4Clip;

        [SerializeField]
        private AudioClip _wave8Clip;

        [SerializeField]
        private float _audioTransitionSeconds = 1.0f;

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

#region Event Handlers
        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            // TODO: set animations

            if(args.WaveIndex == 0) {
                _skinSwapper.SetSkin(0);
                // wave 0 music started by the game state
            } else if(args.WaveIndex == 4) {
                _skinSwapper.SetSkin(1);
                AudioManager.Instance.TransitionMusic(_wave4Clip, _audioTransitionSeconds);
            }  else if(args.WaveIndex == 8) {
                _skinSwapper.SetSkin(2);
                AudioManager.Instance.TransitionMusic(_wave8Clip, _audioTransitionSeconds);
            }
        }
#endregion
    }
}
