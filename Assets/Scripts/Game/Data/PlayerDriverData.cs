using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class PlayerDriverData : ScriptableObject
    {
        [SerializeField]
        private float _movementLerpSpeed = 5.0f;

        public float MovementLerpSpeed => _movementLerpSpeed;

        [SerializeField]
        private float _lookLerpSpeed = 10.0f;

        public float LookLerpSpeed => _lookLerpSpeed;
    }
}
