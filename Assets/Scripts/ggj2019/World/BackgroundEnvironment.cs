using pdxpartyparrot.Core.Effects;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    public sealed class BackgroundEnvironment : Environment
    {
        [SerializeField]
        private EffectTrigger _thunderEffect;

        protected override void OnWaveAnimationSet(int waveIndex, TrackEntry trackEntry)
        {
            base.OnWaveAnimationSet(waveIndex, trackEntry);

            trackEntry.Event += OnThunder;
        }

        private void OnThunder(TrackEntry trackEntry, Spine.Event evt)
        {
            if(evt.Data.Name != "thunder") {
                return;
            }

            _thunderEffect.Trigger();
        }
    }
}
