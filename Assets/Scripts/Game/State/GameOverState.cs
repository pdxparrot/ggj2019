using System;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Time;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class GameOverState : SubGameState
    {
        [SerializeField]
        private float _completeWaitTimeSeconds = 5.0f;

        private ITimer _completeTimer;

        [SerializeField]
        private AudioClip _endGameMusic;

        public override void OnEnter()
        {
            base.OnEnter();

            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.PlayMusic(_endGameMusic);

            _completeTimer = TimeManager.Instance.AddTimer();
            _completeTimer.TimesUpEvent += CompleteTimerTimesUpEventHandler;
            _completeTimer.Start(_completeWaitTimeSeconds);
        }

        public override void OnExit()
        {
            TimeManager.Instance.RemoveTimer(_completeTimer);
            _completeTimer = null;

            AudioManager.Instance.StopAllMusic();

            base.OnExit();
        }

        public virtual void Initialize()
        {
        }

#region Event Handlers
        private void CompleteTimerTimesUpEventHandler(object sender, EventArgs args)
        {
            GameStateManager.Instance.TransitionToInitialStateAsync();
        }
#endregion
    }
}
