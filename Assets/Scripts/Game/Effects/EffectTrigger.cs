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

        private Coroutine _effectWaiter;

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
                _vfx.Stop(true);
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

            _effectWaiter = StartCoroutine(EffectWaiter(callback));
        }

        // forcefully stops the trigger early
        public void StopTrigger()
        {
            if(null != _effectWaiter) {
                StopCoroutine(_effectWaiter);
                _effectWaiter = null;
            }

            if(null != _audioSource) {
                _audioSource.Stop();
            }

            if(null != _vfx) {
                _vfx.Stop(true);
                _vfx.time = 0.0f;
            }
        }

        // resets the effects
        public void Reset()
        {
            if(null != _vfx) {
                _vfx.Clear(true);
                _vfx.Simulate(0.0f, true, true);
            }
        }

        private IEnumerator EffectWaiter(Action callback)
        {
            WaitForSeconds wait = new WaitForSeconds(0.05f);
            while(IsVfxPlaying || IsAudioPlaying) {
                yield return wait;
            }

            _effectWaiter = null;
            callback?.Invoke();
        }
    }
}
