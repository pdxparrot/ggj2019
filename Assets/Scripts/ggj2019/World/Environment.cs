using System;

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
        private WaveEnvironmentSwapper[] _environmentSwappers;

        private SpineSkinSwapper _skinSwapper;

        private SpineAnimationHelper _spineAnimationHelper;

#region Unity Lifecycle
        private void Awake()
        {
            _skinSwapper = GetComponent<SpineSkinSwapper>();
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
                    _skinSwapper.SetSkin(swapper.Skin);
                    TrackEntry trackEntry = _spineAnimationHelper.SetAnimation(swapper.Animation, true);
                    OnWaveAnimationSet(args.WaveIndex, trackEntry);
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
