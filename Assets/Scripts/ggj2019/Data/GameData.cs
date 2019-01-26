using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.State;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="GameData", menuName="pdxpartyparrot/ggj2019/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        [SerializeField]
        private LayerMask _viewerLayer;

        public LayerMask ViewerLayer => _viewerLayer;

        [Space(10)]

        [Header("Game Mode States")]

        [SerializeField]
        private FFAGameState _ffaGameStatePrefab;

        public FFAGameState FFAGameStatePrefab => _ffaGameStatePrefab;

        [SerializeField]
        private TeamsGameState _teamsGameStatePrefab;

        public TeamsGameState TeamsGameStatePrefab => _teamsGameStatePrefab;

        [Space(10)]

#region Viewers
        [Header("Viewer")]

        [SerializeField]
        private StaticViewer _viewerPrefab;

        public StaticViewer ViewerPrefab => _viewerPrefab;

        [SerializeField]
        private Vector3 _viewerPosition;

        public Vector3 ViewerPosition => _viewerPosition;
#endregion
    }
}
