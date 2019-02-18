using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.State;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="GameData", menuName="pdxpartyparrot/ggj2019/Data/Game Data")]
    [Serializable]
    public sealed class GameData : Game.Data.GameData
    {
        [Space(10)]

        [SerializeField]
        [FormerlySerializedAs("_ffaGameStatePrefab")]
        private FFAGameState _mainGameStatePrefab;

        public FFAGameState MainGameStatePrefab => _mainGameStatePrefab;

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

#region Hive
        [Header("Hive")]

        [SerializeField]
        private int _hiveArmorHealth = 2;

        public int HiveArmorHealth => _hiveArmorHealth;

        [SerializeField]
        private int _minBees = 6;

        public int MinBees => _minBees;

        public int BeePoolSize => MinBees * 4;

        [SerializeField]
        private float _beeSpawnCooldown = 10.0f;

        public float BeeSpawnCooldown => _beeSpawnCooldown;

        [SerializeField]
        private NPCBeeData _beeData;

        public NPCBeeData BeeData => _beeData;

        [SerializeField]
        private NPCBee _beePrefab;

        public NPCBee BeePrefab => _beePrefab;
#endregion

        [Space(10)]

        [SerializeField]
        private float _enemySpawnImmunitySeconds = 1.0f;

        public float EnemySpawnImmunitySeconds => _enemySpawnImmunitySeconds;

        [SerializeField]
        private WaveSpawner[] _waveSpawnerPrefabs;

        public WaveSpawner[] WaveSpawnerPrefabs => _waveSpawnerPrefabs;

        [SerializeField]
        private CollectableData _collectableData;

        public CollectableData CollectableData => _collectableData;

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
