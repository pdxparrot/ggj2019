using System;

using pdxpartyparrot.Core.Camera;

using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField]
        [FormerlySerializedAs("_viewerPrefab")]
        private Viewer _playerViewerPrefab;

        public Viewer PlayerViewerPrefab => _playerViewerPrefab;
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
    }
}
