using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class TimerEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private float _seconds;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _timer = new Timer();

        public override bool WaitForComplete => true;

        public override bool IsDone => !_timer.IsRunning;

        public override void OnStart()
        {
            _timer.Start(_seconds);
        }

        public override void OnStop()
        {
            _timer.Stop();
        }

        public override void OnUpdate(float dt)
        {
            _timer.Update(dt);
        }
    }
}
