using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.World;
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

#region Game Mode States
        [Space(10)]

        [Header("Game Mode States")]

        [SerializeField]
        private FFAGameState _ffaGameStatePrefab;

        public FFAGameState FFAGameStatePrefab => _ffaGameStatePrefab;
#endregion

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

        [Space(10)]

#region Game
        [Header("Game")]

        [SerializeField]
        private int _hiveArmorHealth = 2;

        public int HiveArmorHealth => _hiveArmorHealth;

        [SerializeField]
        private WaveSpawner _waveSpawnerPrefab;

        public WaveSpawner WaveSpawnerPrefab => _waveSpawnerPrefab;

        [SerializeField]
        private CollectableData _collectableData;

        public CollectableData CollectableData => _collectableData;

        [SerializeField]
        private NPCBeeData _beeData;

        public NPCBeeData BeeData => _beeData;
#endregion

        [Space(10)]

#region Score
        [Header("Score")]

        [SerializeField]
        private int _deathPenalty;

        public int DeathPenalty => _deathPenalty;

        [SerializeField]
        private int _hiveDamagePenalty;

        public int HiveDamagePenalty => _hiveDamagePenalty;

        [SerializeField]
        private int _pollenScore;

        public int PollenScore => _pollenScore;

        [SerializeField]
        private int _beetleScore;

        public int BeetleScore => _beetleScore;

        [SerializeField]
        private int _waspScore;

        public int WaspScore => _waspScore;
#endregion
    }
}
