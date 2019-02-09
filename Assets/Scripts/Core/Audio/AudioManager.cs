using System;
using System.Collections;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Audio;

namespace pdxpartyparrot.Core.Audio
{
    public sealed class AudioManager : SingletonBehavior<AudioManager>
    {
        // config keys
        private const string MasterVolumeKey = "audio.volume.master";
        private const string MusicVolumeKey = "audio.volume.music";
        private const string SFXVolumeKey = "audio.volume.sfx";
        private const string AmbientVolumeKey = "audio.volume.ambient";

        [SerializeField]
        private AudioMixer _mixer;

        public AudioMixer Mixer => _mixer;

#region Mixer Groups
        [Header("Mixer Groups")]

        [SerializeField]
        private string _musicMixerGroupName = "Music";

        [SerializeField]
        private string _sfxMixerGroupName = "SFX";

        [SerializeField]
        private string _ambientMixerGroupName = "Ambient";
#endregion

        [Space(10)]

#region Attributes
        [Header("Parameters")]

        [SerializeField]
        private string _masterVolumeParameter = "MasterVolume";

        [SerializeField]
        private string _musicVolumeParameter = "MusicVolume";

        [SerializeField]
        private string _sfxVolumeParameter = "SFXVolume";

        [SerializeField]
        private string _ambientVolumeParameter = "AmbientVolume";
#endregion

        [Space(10)]

#region SFX
        [Header("SFX")]

        [SerializeField]
        private AudioSource _oneShotAudioSource;
#endregion

        [Space(10)]

#region Music
        [Header("Music")]

        [SerializeField]
        private AudioSource _music1AudioSource;

        [SerializeField]
        private AudioSource _music2AudioSource;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _musicCrossFade;

        // 0 == music1, 1 = music2
        public float MusicCrossFade
        {
            get => _musicCrossFade;
            set => _musicCrossFade = Mathf.Clamp01(value);
        }

        [SerializeField]
        private float _updateCrossfadeUpdateMs = 100.0f;

        private float UpdateCrossfadeUpdateS => _updateCrossfadeUpdateMs / 1000.0f;

        [SerializeField]
        private float _updateMusicTransitionMs = 100.0f;

        private float UpdateMusicTransitionS => _updateMusicTransitionMs / 1000.0f;

        private Coroutine _musicTransitionRoutine;
#endregion

        [Space(10)]

#region Ambient
        [Header("Ambient")]

        [SerializeField]
        private AudioSource _ambientAudioSource;
#endregion

        [Space(10)]

#region Volume
        [Header("Volume")]

        [SerializeField]
        [ReadOnly]
        private bool _mute;

        public bool Mute
        {
            get => _mute;

            set
            {
                _mute = value;
                Mixer.SetFloat(_masterVolumeParameter, _mute ? 0.0f : MasterVolume);
            }
        }

        public float MasterVolume
        {
            get => PartyParrotManager.Instance.GetFloat(MasterVolumeKey, Mixer.GetFloatOrDefault(_masterVolumeParameter));

            set
            {
                value = Mathf.Clamp(value, -80.0f, 20.0f);

                Mixer.SetFloat(_masterVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(MasterVolumeKey, value);

                Mute = false;
            }
        }

        public float MusicVolume
        {
            get => PartyParrotManager.Instance.GetFloat(MusicVolumeKey, Mixer.GetFloatOrDefault(_musicVolumeParameter, -5.0f));

            set
            {
                value = Mathf.Clamp(value, -80.0f, 20.0f);

                Mixer.SetFloat(_musicVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(MusicVolumeKey, value);

                Mute = false;
            }
        }

        public float SFXVolume
        {
            get => PartyParrotManager.Instance.GetFloat(SFXVolumeKey, Mixer.GetFloatOrDefault(_sfxVolumeParameter));

            set
            {
                value = Mathf.Clamp(value, -80.0f, 20.0f);

                Mixer.SetFloat(_sfxVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(SFXVolumeKey, value);

                Mute = false;
            }
        }

        public float AmbientVolume
        {
            get => PartyParrotManager.Instance.GetFloat(AmbientVolumeKey, Mixer.GetFloatOrDefault(_ambientVolumeParameter, -10.0f));

            set
            {
                value = Mathf.Clamp(value, -80.0f, 20.0f);

                Mixer.SetFloat(_ambientVolumeParameter, value);
                PartyParrotManager.Instance.SetFloat(AmbientVolumeKey, value);

                Mute = false;
            }
        }
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            InitSFXAudioMixerGroup(_oneShotAudioSource);

            InitAudioMixerGroup(_music1AudioSource, _musicMixerGroupName);
            _music1AudioSource.loop = true;

            InitAudioMixerGroup(_music2AudioSource, _musicMixerGroupName);
            _music2AudioSource.loop = true;

            InitAudioMixerGroup(_ambientAudioSource, _ambientMixerGroupName);
            _ambientAudioSource.loop = true;

            // this ensures we've loaded the volumes from the config
            MasterVolume = MasterVolume;
            MusicVolume = MusicVolume;
            SFXVolume = SFXVolume;
            AmbientVolume = AmbientVolume;

            InitDebugMenu();
        }

        private void Start()
        {
            StartCoroutine(UpdateMusicCrossfade());
        }

        protected override void OnDestroy()
        {
            StopAmbient();
            StopAllMusic();

            base.OnDestroy();
        }
#endregion

        public void InitSFXAudioMixerGroup(AudioSource source)
        {
            InitAudioMixerGroup(source, _sfxMixerGroupName);
        }

        private void InitAudioMixerGroup(AudioSource source, string mixerGroupName)
        {
            var mixerGroups = _mixer.FindMatchingGroups(mixerGroupName);
            source.outputAudioMixerGroup = mixerGroups.Length > 0 ? mixerGroups[0] : _mixer.outputAudioMixerGroup;
        }

        public void InitAmbientAudioMixerGroup(AudioSource source)
        {
            InitAudioMixerGroup(source, _ambientMixerGroupName);
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            _oneShotAudioSource.PlayOneShot(audioClip);
        }

        // Plays a music clip on the first audio source at no crossfade
        public void PlayMusic(AudioClip musicAudioClip)
        {
            StopAllMusic();

            _music1AudioSource.clip = musicAudioClip;
            _music1AudioSource.Play();

            MusicCrossFade = 0.0f;
        }

        // Plays a music clip on the second audio source at full crossfade
        public void PlayMusic2(AudioClip musicAudioClip)
        {
            StopAllMusic();

            _music2AudioSource.clip = musicAudioClip;
            _music2AudioSource.Play();

            MusicCrossFade = 1.0f;
        }

        // Plays 2 music clips at 50% crossfade
        public void PlayMusic(AudioClip music1AudioClip, AudioClip music2AudioClip)
        {
            StopAllMusic();

            _music1AudioSource.clip = music1AudioClip;
            _music1AudioSource.Play();

            _music2AudioSource.clip = music2AudioClip;
            _music2AudioSource.Play();

            MusicCrossFade = 0.5f;
        }

        // if stopOnComplete is true, will stop the clip being transitioned away from
        public void TransitionMusic(AudioClip musicAudioClip, float seconds, bool stopOnComplete=true)
        {
            if(null == musicAudioClip) {
                return;
            }

            if(_music1AudioSource.isPlaying && _music2AudioSource.isPlaying) {
                Debug.LogWarning("Attempt to transition music with 2 clips playing");
                return;
            }

            bool transitionToSecond = true;
            float targetCrossfade = 1.0f - _musicCrossFade;
            if(_music1AudioSource.isPlaying) {
                PlayMusic2(musicAudioClip);
                transitionToSecond = true;
            } else {
                PlayMusic(musicAudioClip);
                transitionToSecond = false;
            }

            _musicTransitionRoutine = StartCoroutine(MusicTransitionRoutine(targetCrossfade, seconds, () => {
                if(stopOnComplete) {
                    if(transitionToSecond) {
                        StopMusic();
                    } else {
                        StopMusic2();
                    }
                }
            }));
        }

        public void StopMusic()
        {
            _music1AudioSource.Stop();
        }

        public void StopMusic2()
        {
            _music2AudioSource.Stop();
        }

        public void StopAllMusic()
        {
            if(null != _musicTransitionRoutine) {
                StopCoroutine(_musicTransitionRoutine);
                _musicTransitionRoutine = null;
            }

            StopMusic();
            StopMusic2();
        }

        public void PlayAmbient(AudioClip audioClip)
        {
            StopAmbient();

            _ambientAudioSource.clip = audioClip;
            _ambientAudioSource.Play();
        }

        public void StopAmbient()
        {
            _ambientAudioSource.Stop();
        }

        private IEnumerator UpdateMusicCrossfade()
        {
            WaitForSeconds wait = new WaitForSeconds(UpdateCrossfadeUpdateS);
            while(true) {
                _music1AudioSource.volume = 1.0f - _musicCrossFade;
                _music2AudioSource.volume = _musicCrossFade;

                yield return wait;
            }
        }

        private IEnumerator MusicTransitionRoutine(float targetCrossfade, float seconds, Action onComplete)
        {
            float pct = 0.0f;
            float startCrossfade = MusicCrossFade;

            WaitForSeconds wait = new WaitForSeconds(UpdateMusicTransitionS);
            while(true) {
                MusicCrossFade = Mathf.Lerp(startCrossfade, targetCrossfade, pct);
                yield return wait;

                pct += UpdateMusicTransitionS / seconds;
                if(pct >= 1.0f) {
                    MusicCrossFade = targetCrossfade;
                    break;
                }
            }

            _musicTransitionRoutine = null;
            onComplete?.Invoke();
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.AudioManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Volume", GUI.skin.box);
                    GUILayout.Label($"Master Volume: {MasterVolume}");
                    GUILayout.Label($"Music Volume: {MusicVolume}");
                    GUILayout.Label($"SFX Volume: {SFXVolume}");
                    GUILayout.Label($"Ambient Volume: {AmbientVolume}");
                    GUILayout.Label($"Mute: {Mute}");
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Music", GUI.skin.box);
                    GUILayout.Label($"Music Crossfade: {MusicCrossFade}");
                    GUILayout.Label($"Transitioning: {null != _musicTransitionRoutine}");
                GUILayout.EndVertical();
            };
        }
    }
}
