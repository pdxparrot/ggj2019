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

        [SerializeField]
        private float _spawnImmunitySeconds = 1.0f;

        public float SpawnImmunitySeconds => _spawnImmunitySeconds;

#region Animation
        [SerializeField]
        private string _idleAnimationName = "bee_hover";

        public string IdleAnimationName => _idleAnimationName;  

        [SerializeField]
        private string _flyingAnimationName = "bee-flight";

        public string FlyingAnimationName => _flyingAnimationName;
#endregion
    }
}
