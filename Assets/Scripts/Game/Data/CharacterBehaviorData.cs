using System;

using pdxpartyparrot.Core.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CharacterBehaviorData", menuName="pdxpartyparrot/Game/Data/CharacterBehavior Data")]
    [Serializable]
    public class CharacterBehaviorData : ActorBehaviorData
    {
        [SerializeField]
        private LayerMask _collisionCheckLayerMask;

        public LayerMask CollisionCheckLayerMask => _collisionCheckLayerMask;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _moveXAxisParam = "InputX";

        public string MoveXAxisParam => _moveXAxisParam;

        [SerializeField]
        private string _moveZAxisParam = "InputZ";

        public string MoveZAxisParam => _moveZAxisParam;

        [SerializeField]
        private string _groundedParam = "Landed";

        public string GroundedParam => _groundedParam;

        [SerializeField]
        private string _fallingParam = "Falling";

        public string FallingParam => _fallingParam;
#endregion

        [Space(10)]

#region Physics
        [Header("Physics")]

        [SerializeField]
        [Range(0, 50)]
        [Tooltip("Move speed in m/s")]
        private float _moveSpeed = 30.0f;

        public float MoveSpeed => _moveSpeed;

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("The animator-based threshold that we consider the character to be running")]
        private float _runThreshold = 0.75f;

        public float RunThreshold => _runThreshold;

        public float RunThresholdSquared => RunThreshold * RunThreshold;

        [SerializeField]
        [Range(0, 500)]
        [Tooltip("Add this many m/s to the player's fall speed, to make movement feel better without changing actual gravity")]
        private float _fallSpeedAdjustment = 200.0f;

        public float FallSpeedAdjustment => _fallSpeedAdjustment;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("The characters terminal velocity in m/s")]
        private float _terminalVelocity = 50.0f;

        public float TerminalVelocity => _terminalVelocity;

        [SerializeField]
        [Tooltip("Max distance from the ground that the character is considered grounded")]
        private float _GroundedEpsilon = 0.1f;

        public float GroundedEpsilon => _GroundedEpsilon;

        [SerializeField]
        [Tooltip("The length of the ground check sphere cast (useful for checking actual slope below the character)")]
        private float _groundCheckLength = 1.0f;

        public float GroundCheckLength => _groundCheckLength;

        [SerializeField]
        private float _slopeLimit = 30.0f;

        public float SlopeLimit => _slopeLimit;

        [SerializeField]
        [Tooltip("Allow movement while not grounded")]
        private bool _allowAirControl = true;

        public bool AllowAirControl => _allowAirControl;
#endregion
    }
}
