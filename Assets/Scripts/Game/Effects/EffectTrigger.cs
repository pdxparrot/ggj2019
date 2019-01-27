using System;
using System.Collections;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Audio;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects
{
    public sealed class EffectTrigger : MonoBehaviour
    {
        [SerializeField]
        [CanBeNull]
        private ParticleSystem _vfx;

        [SerializeField]
        [CanBeNull]
        private AudioClip _audioClip;

        [SerializeField]
        [CanBeNull]
        private AudioSource _audioSource;

        private bool IsVfxPlaying => null != _vfx && _vfx.isPlaying;

        private bool IsAudioPlaying => null != _audioSource && _audioSource.isPlaying;

// TODO: add animations to this

#region Unity Lifecycle
        private void Awake()
        {
            if(null != _audioSource) {
                AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);
            }
        }

        private void Start()
        {
            if(null != _vfx) {
                _vfx.Stop();
            }
        }
#endregion

        public void Trigger(Action callback=null)
        {
            if(null != _vfx) {
                _vfx.Play();
            }

            if(null == _audioSource) {
                AudioManager.Instance.PlayOneShot(_audioClip);
            } else {
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            }

            StartCoroutine(EffectWaiter(callback));
        }

        private IEnumerator EffectWaiter(Action callback) {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            while(IsVfxPlaying || IsAudioPlaying) {
                yield return wait;
            }
            callback?.Invoke();
        }
    }
}
