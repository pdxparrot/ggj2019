using System;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Camera;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class PlayerData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _playerLayer;

        public LayerMask PlayerLayer => _playerLayer;

        [SerializeField]
        private LayerMask _viewerLayer;

        public LayerMask ViewerLayer => _viewerLayer;

        [Space(10)]

#region Viewers
        [Header("Viewer")]

        [SerializeField]
        private Viewer _playerViewerPrefab;

        public IPlayerViewer PlayerViewerPrefab => (IPlayerViewer)_playerViewerPrefab;
#endregion

        [Space(10)]

// TODO: this could probably go into an Actor/Character data object

#region Physics
        [Header("Physics")]

        [SerializeField]
        private float _mass = 1.0f;

        public float Mass => _mass;

        [SerializeField]
        private float _drag = 0.0f;

        public float Drag => _drag;

        [SerializeField]
        private float _angularDrag = 0.0f;

        public float AngularDrag => _angularDrag;
#endregion

#region Controls
        [Header("Controls")]

        [SerializeField]
        private float _movementLerpSpeed = 1.0f;

        public float MovementLerpSpeed => _movementLerpSpeed;

        [SerializeField]
        private float _lookLerpSpeed = 1.0f;

        public float LookLerpSpeed => _lookLerpSpeed;
#endregion
    }
}
