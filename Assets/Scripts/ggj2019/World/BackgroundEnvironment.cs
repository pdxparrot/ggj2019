using pdxpartyparrot.Core.Effects;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    public sealed class BackgroundEnvironment : Environment
    {
        [SerializeField]
        private EffectTrigger _thunderEffect;

        [SerializeField]
        private string _thunderEventName = "thunder";

        protected override void OnWaveAnimationSet(int waveIndex, TrackEntry trackEntry)
        {
            base.OnWaveAnimationSet(waveIndex, trackEntry);

            trackEntry.Event += OnThunder;
        }

        private void OnThunder(TrackEntry trackEntry, Spine.Event evt)
        {
            if(evt.Data.Name == _thunderEventName) {
                _thunderEffect.Trigger();
            }
        }
    }
}
