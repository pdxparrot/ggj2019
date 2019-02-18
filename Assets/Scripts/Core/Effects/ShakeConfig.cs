using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects
{
    [Serializable]
    public class ShakeConfig
    {
        [SerializeField]
        private float _duration = 1.0f;

        public float Duration => _duration;

        [SerializeField]
        private Vector3 _strength = new Vector3(1.0f, 1.0f, 1.0f);

        public Vector3 Strength => _strength;

        [SerializeField]
        private int _vibrato = 1;

        public int Vibrato => _vibrato;

        [SerializeField]
        [Range(0.0f, 180.0f)]
        [Tooltip("Values over 90 kind of suck")]
        private float _randomness = 45.0f;

        public float Randomness => _randomness;

        [SerializeField]
        private bool _fadeOut = true;

        public bool FadeOut => _fadeOut;

        public ShakeConfig(float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut=true)
        {
            _duration = duration;
            _strength = strength;
            _vibrato = vibrato;
            _randomness = randomness;
            _fadeOut = fadeOut;
        }

        public ShakeConfig(float duration, float strength, int vibrato, float randomness, bool fadeOut=true)
        {
            _duration = duration;
            _strength = new Vector3(strength, strength, strength);
            _vibrato = vibrato;
            _randomness = randomness;
            _fadeOut = fadeOut;
        }
    }
}
