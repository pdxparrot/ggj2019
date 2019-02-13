using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ParticleSystemEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private ParticleSystem _vfx;

        public override bool IsDone => !_vfx.isPlaying;

        public override void Initialize()
        {
            // in case the VFX is set to auto-play
            OnStop();
        }

        public override void OnStart()
        {
            _vfx.Play();
        }

        public override void OnStop()
        {
            _vfx.Stop(true);
            _vfx.time = 0.0f;
        }

        public override  void OnReset()
        {
            _vfx.Clear(true);
            _vfx.Simulate(0.0f, true, true);
        }
    }
}
