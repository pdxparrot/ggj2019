using System;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    [RequireComponent(typeof(SpineSkinHelper))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public class Environment : MonoBehaviour
    {
        [SerializeField]
        private WaveEnvironmentSwapper[] _environmentSwappers;

        [SerializeField]
        private EffectTrigger _fadeOutEffectTrigger;

        [SerializeField]
        private EffectTrigger _fadeInEffectTrigger;

        private SpineSkinHelper _skinHelper;

        private SpineAnimationHelper _spineAnimationHelper;

#region Unity Lifecycle
        private void Awake()
        {
            _skinHelper = GetComponent<SpineSkinHelper>();
            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();

            GameManager.Instance.GameStartEvent += GameStartEventHandler;
            GameManager.Instance.GameEndEvent += GameEndEventHandler;
        }

        private void OnDestroy()
        {
            if(GameManager.HasInstance) {
                GameManager.Instance.GameEndEvent -= GameEndEventHandler;
                GameManager.Instance.GameStartEvent -= GameStartEventHandler;
            }
        }
#endregion

        private void SwapSkin(WaveEnvironmentSwapper swapper, int waveIndex)
        {
            _skinHelper.SetSkin(swapper.Skin);

            TrackEntry trackEntry = _spineAnimationHelper.SetAnimation(swapper.Animation, true);
            OnWaveAnimationSet(waveIndex, trackEntry);
        }

#region Event Handlers
        private void GameStartEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.WaveSpawner.WaveStartEvent += WaveStartEventHandler;
        }

        private void GameEndEventHandler(object sender, EventArgs args)
        {
            GameManager.Instance.WaveSpawner.WaveStartEvent -= WaveStartEventHandler;
        }

        private void WaveStartEventHandler(object sender, SpawnWaveEventArgs args)
        {
            foreach(WaveEnvironmentSwapper swapper in _environmentSwappers) {
                if(args.WaveIndex == swapper.Wave) {
                    if(swapper.Fade && null != _fadeOutEffectTrigger && null != _fadeInEffectTrigger) {
                        _fadeOutEffectTrigger.Trigger(() => {
                            SwapSkin(swapper, args.WaveIndex);
                            _fadeInEffectTrigger.Trigger();
                        });
                    } else {
                        SwapSkin(swapper, args.WaveIndex);
                    }
                }
            }
        }

        protected virtual void OnWaveAnimationSet(int waveIndex, TrackEntry trackEntry)
        {
            //trackEntry.Event += OnAnimationEvent;
        }

        private void OnAnimationEvent(TrackEntry trackEntry, Spine.Event evt)
        {
            Debug.Log($"Animation event: '{evt.Data.Name}'");
        }
#endregion
    }
}
