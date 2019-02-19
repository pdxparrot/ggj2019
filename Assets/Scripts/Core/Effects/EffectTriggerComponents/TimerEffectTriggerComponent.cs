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

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            _timer.Update(dt);
        }
#endregion

        public override void OnStart()
        {
            _timer.Start(_seconds);
        }

        public override void OnStop()
        {
            _timer.Stop();
        }
    }
}
