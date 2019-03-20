using System;

using pdxpartyparrot.Core.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="HiveBehaviorData", menuName="pdxpartyparrot/ggj2019/Data/HiveBehavior Data")]
    [Serializable]
    public sealed class HiveBehaviorData : ActorBehaviorData
    {
        [Space(10)]

        [Header("Armor")]

        [SerializeField]
        private int _hiveArmorHealth = 2;

        public int HiveArmorHealth => _hiveArmorHealth;

        [Header("Bees")]

        [SerializeField]
        [Tooltip("Minimum number of bees to keep in the world, per-player")]
        private int _minBees = 6;

        public int MinBees => _minBees;

        [SerializeField]
        private float _beeSpawnCooldown = 10.0f;

        public float BeeSpawnCooldown => _beeSpawnCooldown;
    }
}
