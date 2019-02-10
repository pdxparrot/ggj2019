using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Input
{
    [Serializable]
    public struct RumbleConfig
    {
        [SerializeField]
        [Range(0.0f, 5.0f)]
        private float _seconds;

        public float Seconds => _seconds;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _lowFrequency;

        public float LowFrequency => _lowFrequency;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _highFrequency;

        public float HighFrequency => _highFrequency;
    }
}
