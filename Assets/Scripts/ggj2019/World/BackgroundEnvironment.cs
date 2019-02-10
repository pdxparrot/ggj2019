using pdxpartyparrot.Core.Audio;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    public sealed class BackgroundEnvironment : Environment
    {
        [SerializeField]
        private AudioClip _thunderAudioClip;

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
            AudioManager.Instance.PlayOneShot(_thunderAudioClip);
        }
    }
}
