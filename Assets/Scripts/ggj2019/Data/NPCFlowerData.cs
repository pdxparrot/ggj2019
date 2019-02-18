using System;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCFlowerData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Flower Data")]
    [Serializable]
    public sealed class NPCFlowerData : NPCEnemyData
    {
        [SerializeField]
        private int _pollen = 5;

        public int Pollen => _pollen;

        [SerializeField]
        private FloatRange _pollenSpawnCooldown = new FloatRange(1.0f, 5.0f);

        public FloatRange PollenSpawnCooldown => _pollenSpawnCooldown;

        [SerializeField]
        private CollectableData _pollenData;

        public CollectableData PollenData => _pollenData;

        [SerializeField]
        private int _maxPollen = 2;

        public int MaxPollen => _maxPollen;

#region Animations
        [SerializeField]
        private string _idleAnimationName = "flower_idle";

        public string IdleAnimationName => _idleAnimationName;
#endregion
    }
}
