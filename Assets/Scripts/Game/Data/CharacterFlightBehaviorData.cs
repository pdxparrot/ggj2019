using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CharacterFlightBehaviorData", menuName="pdxpartyparrot/Game/Data/CharacterFlightBehavior Data")]
    [Serializable]
    public class CharacterFlightBehaviorData : ScriptableObject
    {
#region Movement
        [Header("Movement")]

        [SerializeField]
        private float _maxAttackAngle = 45.0f;

        public float MaxAttackAngle => _maxAttackAngle;

        [SerializeField]
        private float _maxBankAngle = 45.0f;

        public float MaxBankAngle => _maxBankAngle;

        [SerializeField]
        private float _rotationAnimationSpeed = 5.0f;

        public float RotationAnimationSpeed => _rotationAnimationSpeed;

        [SerializeField]
        private float _linearThrust = 10.0f;

        public float LinearThrust => _linearThrust;

        [SerializeField]
        private float _turnSpeed = 10.0f;

        public float TurnSpeed => _turnSpeed;

        [SerializeField]
        private float _terminalVelocity = 10.0f;

        public float TerminalVelocity => _terminalVelocity;
#endregion
    }
}
