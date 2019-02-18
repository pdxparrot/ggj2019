using System;

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

#region Animations
        [SerializeField]
        private string _idleAnimationName = "flower_idle";

        public string IdleAnimationName => _idleAnimationName;
#endregion
    }
}
