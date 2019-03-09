﻿using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;

using UnityEngine;

// TODO: move into TimeManager and have the TimeManger manage the timers
// also having named timers would be good (so they can be shared and what not)
// which probably means it should use events rather than holding an action
namespace pdxpartyparrot.Core.Time
{
    [Serializable]
    public class Timer
    {
        [SerializeField]
        [ReadOnly]
        private float _timerSeconds;

        public float TimerSeconds => _timerSeconds;

        [SerializeField]
        [ReadOnly]
        private float _secondsRemaining;

        public float SecondsRemaining => _secondsRemaining;

        [SerializeField]
        [ReadOnly]
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        [CanBeNull]
        private Action _onTimesUp;

        public void Start(float timerSeconds, Action onTimesUp=null)
        {
            _onTimesUp = onTimesUp;
            _timerSeconds = timerSeconds;
            _secondsRemaining = _timerSeconds;
            _isRunning = true;
        }

        public void Start(Range timerSeconds, Action onTimesUp=null)
        {
            _onTimesUp = onTimesUp;
            _timerSeconds = timerSeconds.GetRandomValue();
            _secondsRemaining = _timerSeconds;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Continue()
        {
            _isRunning = true;
        }

        public void AddTime(float seconds)
        {
            _secondsRemaining += seconds;
        }

        public void Update(float dt)
        {
            if(PartyParrotManager.Instance.IsPaused || !_isRunning) {
                return;
            }

            _secondsRemaining -= dt;
            if(_secondsRemaining <= 0.0f) {
                Stop();

                _secondsRemaining = 0.0f;

                _onTimesUp?.Invoke();
            }
        }
    }
}