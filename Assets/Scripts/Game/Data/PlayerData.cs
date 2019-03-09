using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class PlayerData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _playerLayer;

        public LayerMask PlayerLayer => _playerLayer;

        [Space(10)]

// TODO: this could probably go into an Actor/Character data object

        [SerializeField]
        private CharacterBehaviorData _playerControllerData;

        public CharacterBehaviorData PlayerControllerData => _playerControllerData;

#region Physics
        [Header("Physics")]

        [SerializeField]
        private float _mass = 45.0f;

        public float Mass => _mass;

        [SerializeField]
        private float _drag = 0.0f;

        public float Drag => _drag;

        [SerializeField]
        private float _angularDrag = 0.0f;

        public float AngularDrag => _angularDrag;

        [SerializeField]
        private bool _isKinematic = false;

        public bool IsKinematic => _isKinematic;
#endregion

#region Controls
        [Header("Controls")]

        [SerializeField]
        private float _movementLerpSpeed = 5.0f;

        public float MovementLerpSpeed => _movementLerpSpeed;

        [SerializeField]
        private float _lookLerpSpeed = 10.0f;

        public float LookLerpSpeed => _lookLerpSpeed;
#endregion
    }
}
