using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Time
{
    [Serializable]
    public class Stopwatch
    {
        [SerializeField]
        [ReadOnly]
        private float _stopwatchSeconds;

        public float StopwatchSeconds => _stopwatchSeconds;

        [SerializeField]
        [ReadOnly]
        private bool _isRunning;

        public bool IsRunning => _isRunning;

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Reset()
        {
            _stopwatchSeconds = 0.0f;
        }

        public void Update(float dt)
        {
            if(PartyParrotManager.Instance.IsPaused || !_isRunning) {
                return;
            }

            _stopwatchSeconds += dt;
        }
    }
}
