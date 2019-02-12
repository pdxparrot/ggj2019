using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCBeetleData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Beetle Data")]
    [Serializable]
    public sealed class NPCBeetleData : NPCEnemyData
    {
        [SerializeField]
        private float _harvestCooldown = 1.0f;

        public float HarvestCooldown => _harvestCooldown;

#region Animations
        [SerializeField]
        private string _idleAnimation = "beetle_idle";

        public string IdleAnimation => _idleAnimation;

        [SerializeField]
        private string _harvestAnimation = "beetle_idle";

        public string HarvestAnimation => _harvestAnimation;
#endregion
    }
}
