﻿using System;

using UnityEngine;
using UnityEngine.Audio;

namespace pdxpartyparrot.Core.Data
{
    [CreateAssetMenu(fileName="AudioData", menuName="pdxpartyparrot/Core/Data/Audio Data")]
    [Serializable]
    public class AudioData : ScriptableObject
    {
        [SerializeField]
        private AudioMixer _mixer;

        public AudioMixer Mixer => _mixer;

        [Space(10)]

#region Mixer Groups
        [Header("Mixer Groups")]

        [SerializeField]
        private string _musicMixerGroupName = "Music";

        public string MusicMixerGroupName => _musicMixerGroupName;

        [SerializeField]
        private string _sfxMixerGroupName = "SFX";

        public string SFXMixerGroupName => _sfxMixerGroupName;

        [SerializeField]
        private string _ambientMixerGroupName = "Ambient";

        public string AmbientMixerGroupName => _ambientMixerGroupName;
#endregion

        [Space(10)]

#region Mixer Parameters
        [Header("Mixer Parameters")]

        [SerializeField]
        private string _masterVolumeParameter = "MasterVolume";

        public string MasterVolumeParameter => _masterVolumeParameter;

        [SerializeField]
        private string _musicVolumeParameter = "MusicVolume";

        public string MusicVolumeParameter => _musicVolumeParameter;

        [SerializeField]
        private string _sfxVolumeParameter = "SFXVolume";

        public string SFXVolumeParameter => _sfxVolumeParameter;

        [SerializeField]
        private string _ambientVolumeParameter = "AmbientVolume";

        public string AmbientVolumeParameter => _ambientVolumeParameter;
#endregion

        [Space(10)]

#region Mixer Snapshots
        [Header("Mixer Snapshots")]

        [SerializeField]
        private string _unpausedSnapshotName = "Unpaused";

        public string UnpausedSnapshotName => _unpausedSnapshotName;

        [SerializeField]
        private string _pausedSnapshotName = "Paused";

        public string PausedSnapshotName => _pausedSnapshotName;
#endregion

        [Space(10)]

        [SerializeField]
        private float _updateCrossfadeUpdateMs = 100.0f;

        public float UpdateCrossfadeUpdateSeconds => _updateCrossfadeUpdateMs / 1000.0f;

        [SerializeField]
        private float _updateMusicTransitionMs = 100.0f;

        public float UpdateMusicTransitionSeconds => _updateMusicTransitionMs / 1000.0f;
    }
}
