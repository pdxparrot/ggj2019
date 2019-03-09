﻿using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CharacterBehaviorData", menuName="pdxpartyparrot/Game/Data/CharacterBehavior Data")]
    [Serializable]
    public class CharacterBehaviorData : ScriptableObject
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

        [SerializeField]
        private string _jumpParam = "Jump";

        public string JumpParam => _jumpParam;

        [SerializeField]
        private string _longJumpParam = "LongJump";

        public string LongJumpParam => _longJumpParam;

        [SerializeField]
        private string _hoverParam = "Hover";

        public string HoverParam => _hoverParam;
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
#endregion

        [Space(10)]

#region Jumping
        [Header("Jumping")]

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("How high does the character jump")]
        private float _jumpHeight = 30.0f;

        public float JumpHeight => _jumpHeight;
#endregion

        [Space(10)]

#region Long Jumping
        [Header("Long Jumping")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How long to hold jump before allowing a long jump")]
        private float _longJumpHoldSeconds = 0.5f;

        public float LongJumpHoldSeconds => _longJumpHoldSeconds;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("How high does the character jump when long jumping")]
        private float _longJumpHeight = 50.0f;

        public float LongJumpHeight => _longJumpHeight;
#endregion

        [Space(10)]

#region Hover
        [Header("Hover")]

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("How long to hold hover before hovering starts")]
        private float _hoverHoldSeconds = 0.5f;

        public float HoverHoldSeconds => _hoverHoldSeconds;

        [SerializeField]
        [Range(0, 60)]
        [Tooltip("Max time hover can last")]
        private float _hoverTimeSeconds = 10.0f;

        public float HoverTimeSeconds => _hoverTimeSeconds;

        [SerializeField]
        [Range(0, 10)]
        private float _hoverCooldownSeconds = 1.0f;

        public float HoverCooldownSeconds => _hoverCooldownSeconds;

        [SerializeField]
        [Range(0, 10)]
        [Tooltip("Seconds of charge to recover every second after cooldown")]
        private float _hoverRechargeRate = 0.5f;

        public float HoverRechargeRate => _hoverRechargeRate;

        [SerializeField]
        [Range(0, 100)]
        [Tooltip("The acceleration caused by hovering")]
        private float _hoverAcceleration = 5.0f;

        public float HoverAcceleration => _hoverAcceleration;

        [SerializeField]
        [Range(0, 100)]
        private float _hoverMoveSpeed = 30.0f;

        public float HoverMoveSpeed => _hoverMoveSpeed;

        [SerializeField]
        private bool _hoverWhenGrounded;

        public bool HoverWhenGrounded => _hoverWhenGrounded;
#endregion

        [Space(10)]

        [SerializeField]
        [Tooltip("Allow movement while not grounded")]
        private bool _allowAirControl = true;

        public bool AllowAirControl => _allowAirControl;
    }
}
