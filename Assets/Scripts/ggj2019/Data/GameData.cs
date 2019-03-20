using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.ggj2019.Collectables;
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

        [Header("Game States")]

        [SerializeField]
        [FormerlySerializedAs("_ffaGameStatePrefab")]
        private MainGameState _mainGameStatePrefab;

        public MainGameState MainGameStatePrefab => _mainGameStatePrefab;

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

        [SerializeField]
        private HiveBehaviorData _hiveBehaviorData;

        public HiveBehaviorData HiveBehaviorData => _hiveBehaviorData;

        [Space(10)]

        [SerializeField]
        private BeeData _beeData;

        public BeeData BeeData => _beeData;

        [SerializeField]
        private Bee _beePrefab;

        public Bee BeePrefab => _beePrefab;

        [SerializeField]
        private int _beePoolSize = 20;

        public int BeePoolSize => _beePoolSize;

        [Space(10)]

        [SerializeField]
        private PollenBehaviorData _pollenBehaviorData;

        public PollenBehaviorData PollenBehaviorData => _pollenBehaviorData;

        [Space(10)]

#region Pollen
        [Header("Pollen")]

        [SerializeField]
        private Pollen _pollenPrefab;

        public Pollen PollenPrefab => _pollenPrefab;

        [SerializeField]
        private int _pollenPoolSize = 20;

        public int PollenPoolSize => _pollenPoolSize;
#endregion

        [Space(10)]

#region NPCS
        [Header("NPCs")]

        [SerializeField]
        private float _enemySpawnImmunitySeconds = 1.0f;

        public float EnemySpawnImmunitySeconds => _enemySpawnImmunitySeconds;

        [SerializeField]
        private WaveSpawner[] _waveSpawnerPrefabs;

        public WaveSpawner[] WaveSpawnerPrefabs => _waveSpawnerPrefabs;
#endregion

        [Space(10)]

#region Floating Text
        [Header("Floating Text")]

        [SerializeField]
        private FloatingText _floatingTextPrefab;

        public FloatingText FloatingTextPrefab => _floatingTextPrefab;

        [SerializeField]
        private int _floatingTextPoolSize = 10;

        public int FloatingTextPoolSize => _floatingTextPoolSize;

        [SerializeField]
        private Color _negativeFloatingTextColor = Color.red;

        public Color NegativeFloatingTextColor => _negativeFloatingTextColor;

        [SerializeField]
        private Color _positiveFloatingTextColor = Color.green;

        public Color PositiveFloatingTextColor => _positiveFloatingTextColor;

        [SerializeField]
        private Color _pollenFloatingTextColor = Color.cyan;

        public Color PollenFloatingTextColor => _pollenFloatingTextColor;
#endregion

        [Space(10)]

#region Score
        [Header("Score")]

        [SerializeField]
        private int _deathPenalty = 10;

        public int DeathPenalty => _deathPenalty;

        [SerializeField]
        private int _hiveDamagePenalty = 10;

        public int HiveDamagePenalty => _hiveDamagePenalty;

        [SerializeField]
        private int _pollenScore = 10;

        public int PollenScore => _pollenScore;

        [SerializeField]
        private int _beetleScore = 10;

        public int BeetleScore => _beetleScore;

        [SerializeField]
        private int _flowerDestroyedPenalty = 10;

        public int FlowerDestroyedPenalty => _flowerDestroyedPenalty;

        [SerializeField]
        private int _waspScore = 10;

        public int WaspScore => _waspScore;
#endregion
    }
}
