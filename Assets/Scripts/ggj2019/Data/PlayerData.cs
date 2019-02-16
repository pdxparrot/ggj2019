using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="PlayerData", menuName="pdxpartyparrot/ggj2019/Data/Player Data")]
    [Serializable]
    public sealed class PlayerData : Game.Data.PlayerData
    {
        [SerializeField]
        private float _respawnSeconds = 1.0f;

        public float RespawnSeconds => _respawnSeconds;
    }
}
