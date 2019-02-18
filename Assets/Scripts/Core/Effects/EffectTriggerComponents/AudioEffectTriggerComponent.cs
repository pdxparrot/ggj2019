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

        [SerializeField]
        private bool _waitForComplete;

        public override bool WaitForComplete => _waitForComplete;

        public override bool IsDone => null == _audioSource || !_audioSource.isPlaying;

        public override void Initialize()
        {
            if(null != _audioSource) {
                AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);
            }
        }

        public override void OnStart()
        {
            if(EffectsManager.Instance.EnableAudio) {
                if(null == _audioSource) {
                    // TODO: set a timer or something to timeout when we'll be done
                    AudioManager.Instance.PlayOneShot(_audioClip);
                } else {
                    _audioSource.clip = _audioClip;
                    _audioSource.Play();
                }
            } else {
                // TODO: set a timer or something to timeout when we'd normally be done
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
