using System;
using System.Collections;
using System.Collections.Generic;

using pdxpartyparrot.Core.Collections;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Time
{
    public sealed class TimeManager : SingletonBehavior<TimeManager>
    {
        private sealed class Timer : ITimer
        {
#region Events
            public event EventHandler StartEvent;
            public event EventHandler StopEvent;
            public event EventHandler TimesUpEvent;
#endregion

            public float TimerSeconds { get; private set; }

            public float SecondsRemaining { get; private set; }

            public bool IsRunning { get; private set; }

            public void Start(float timerSeconds)
            {
                if(IsRunning) {
                    return;
                }

                TimerSeconds = timerSeconds;
                SecondsRemaining = TimerSeconds;
                IsRunning = true;

                StartEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Start(Range timerSeconds)
            {
                if(IsRunning) {
                    return;
                }

                TimerSeconds = timerSeconds.GetRandomValue();
                SecondsRemaining = TimerSeconds;
                IsRunning = true;

                StartEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Stop()
            {
                if(!IsRunning) {
                    return;
                }
                IsRunning = false;

                StopEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Continue()
            {
                if(IsRunning) {
                    return;
                }
                IsRunning = true;

                StartEvent?.Invoke(this, EventArgs.Empty);
            }

            public void AddTime(float seconds)
            {
                SecondsRemaining += seconds;
            }

            public void Update(float dt)
            {
                if(!IsRunning) {
                    return;
                }

                SecondsRemaining -= dt;
                if(SecondsRemaining > 0.0f) {
                    return;
                }
                SecondsRemaining = 0.0f;
                IsRunning = false;

                TimesUpEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private sealed class Stopwatch : IStopwatch
        {
#region Events
            public event EventHandler StartEvent;
            public event EventHandler StopEvent;
            public event EventHandler ResetEvent;
#endregion

            public float StopwatchSeconds { get; private set; }

            public bool IsRunning { get; private set; }

            public void Start()
            {
                if(IsRunning) {
                    return;
                }
                IsRunning = true;

                StartEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Stop()
            {
                if(!IsRunning) {
                    return;
                }
                IsRunning = false;

                StopEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Reset()
            {
                StopwatchSeconds = 0.0f;

                ResetEvent?.Invoke(this, EventArgs.Empty);
            }

            public void Update(float dt)
            {
                if(!IsRunning) {
                    return;
                }

                StopwatchSeconds += dt;
            }
        }

        public static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        public static long SecondsToMilliseconds(float seconds)
        {
            return (long)(seconds * 1000.0f);
        }

        [SerializeField]
        private float _offsetSeconds = 0;

        public double CurrentUnixSeconds => DateTime.UtcNow.Subtract(Epoch).TotalSeconds + _offsetSeconds;

        public long CurrentUnixMs => (long)DateTime.UtcNow.Subtract(Epoch).TotalMilliseconds + SecondsToMilliseconds(_offsetSeconds);

        private readonly HashSet<Timer> _timers = new HashSet<Timer>();
        private readonly Dictionary<string, Timer> _namedTimers = new Dictionary<string, Timer>();

        private readonly HashSet<Stopwatch> _stopwatches = new HashSet<Stopwatch>();
        private readonly Dictionary<string, Stopwatch> _namedStopwatches = new Dictionary<string, Stopwatch>();

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = UnityEngine.Time.deltaTime;

            UpdateStopwatches(dt);

            UpdateTimers(dt);
        }
#endregion

        // NOTE: this ignores the game being paused
        public void RunAfterDelay(float seconds, Action action)
        {
            StartCoroutine(RunAfterDelayRoutine(seconds, action));
        }

#region Timers
        public ITimer AddTimer()
        {
            Timer timer = new Timer();
            _timers.Add(timer);
            return timer;
        }

        public bool RemoveTimer(ITimer timer)
        {
            if(null == timer) {
                return false;
            }

            timer.Stop();
            return _timers.Remove(timer as Timer);
        }

        public ITimer GetNamedTimer(string timerName)
        {
            return _namedTimers.GetOrAdd(timerName);
        }

        public bool RemoveNamedTimer(string timerName)
        {
            Timer timer = _namedTimers.GetOrDefault(timerName);
            if(null == timer) {
                return false;
            }

            timer.Stop();
            return _namedTimers.Remove(timerName);
        }

        private void UpdateTimers(float dt)
        {
            foreach(var timer in _timers) {
                timer.Update(dt);
            }

            foreach(var kvp in _namedTimers) {
                kvp.Value.Update(dt);
            }
        }
#endregion

#region Stopwatches
        public IStopwatch AddStopwatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            _stopwatches.Add(stopwatch);
            return stopwatch;
        }

        public bool RemoveStopwatch(IStopwatch stopwatch)
        {
            if(null == stopwatch) {
                return false;
            }

            stopwatch.Stop();
            return _stopwatches.Remove(stopwatch as Stopwatch);
        }

        public IStopwatch GetNamedStopwatch(string stopwatchName)
        {
            return _namedStopwatches.GetOrAdd(stopwatchName);
        }

        public bool RemoveNamedStopwatch(string stopwatchName)
        {
            Stopwatch stopwatch = _namedStopwatches.GetOrDefault(stopwatchName);
            if(null == stopwatch) {
                return false;
            }

            stopwatch.Stop();
            return _namedStopwatches.Remove(stopwatchName);
        }

        private void UpdateStopwatches(float dt)
        {
            foreach(var stopwatch in _stopwatches) {
                stopwatch.Update(dt);
            }

            foreach(var kvp in _namedStopwatches) {
                kvp.Value.Update(dt);
            }
        }
#endregion

        private IEnumerator RunAfterDelayRoutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.TimeManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.Label($"Current Unix Seconds: {CurrentUnixSeconds}");
                GUILayout.Label($"Current Unix Milliseconds: {CurrentUnixMs}");

                // TODO: print timers

                // TODO: print stopwatches
            };
        }
    }
}
