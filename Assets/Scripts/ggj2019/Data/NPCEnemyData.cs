using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [Serializable]
    public abstract class NPCEnemyData : NPCData
    {
        [SerializeField]
        private int _damage = 1;

        public int Damage => _damage;
    }
}
