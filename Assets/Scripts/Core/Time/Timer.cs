using System;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Time
{
    public interface ITimer
    {
#region Events
        event EventHandler StartEvent;
        event EventHandler StopEvent;
        event EventHandler TimesUpEvent;
#endregion

        float TimerSeconds { get; }

        float SecondsRemaining { get; }

        bool IsRunning { get; }

        void Start(float timerSeconds);

        void Start(Range timerSeconds);

        void Stop();

        void Continue();

        void AddTime(float seconds);
    }
}
