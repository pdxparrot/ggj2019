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
        private float _randomness = 45.0f;

        public float Randomness => _randomness;

        public ShakeConfig(float duration, Vector3 strength, int vibrato, float randomness)
        {
            _duration = duration;
            _strength = strength;
            _vibrato = vibrato;
            _randomness = randomness;
        }

        public ShakeConfig(float duration, float strength, int vibrato, float randomness)
        {
            _duration = duration;
            _strength = new Vector3(strength, strength, strength);
            _vibrato = vibrato;
            _randomness = randomness;
        }
    }
}
