using System;

using pdxpartyparrot.Core.Input;
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
        private RumbleConfig _respawnRumble;

        public RumbleConfig RespawnRumble => _respawnRumble;

        [SerializeField]
        private RumbleConfig _damageRumble;

        public RumbleConfig DamageRumble => _damageRumble;

        [SerializeField]
        private RumbleConfig _deathRumble;

        public RumbleConfig DeathRumble => _deathRumble;

        [SerializeField]
        private RumbleConfig _gameOverRumble;

        public RumbleConfig GameOverRumble => _gameOverRumble;
    }
}
