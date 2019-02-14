using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class AudioEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        [CanBeNull]
        private AudioSource _audioSource;

        public override bool IsDone => null == _audioSource || !_audioSource.isPlaying;

        public override void Initialize()
        {
            if(null != _audioSource) {
                AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);
            }
        }

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableAudio) {
                return;
            }

            if(null == _audioSource) {
                AudioManager.Instance.PlayOneShot(_audioClip);
            } else {
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            }
        }

        public override void OnStop()
        {
            if(null != _audioSource) {
                _audioSource.Stop();
            }
        }
    }
}
