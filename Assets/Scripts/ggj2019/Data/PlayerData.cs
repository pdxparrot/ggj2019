using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="pdxpartyparrot/ggj2019/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : Game.Data.PlayerData
    {
        [Space(10)]

        [SerializeField]
        private float _respawnSeconds = 1.0f;

        public float RespawnSeconds => _respawnSeconds;

        [SerializeField]
        private float _spawnImmunitySeconds = 1.0f;

        public float SpawnImmunitySeconds => _spawnImmunitySeconds;

        [SerializeField]
        private int _maxPollen = -1;

        public int MaxPollen => _maxPollen;
    }
}
