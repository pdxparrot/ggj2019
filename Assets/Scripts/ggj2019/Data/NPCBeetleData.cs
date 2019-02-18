using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCBeetleData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Beetle Data")]
    [Serializable]
    public sealed class NPCBeetleData : NPCEnemyData
    {
        [SerializeField]
        [FormerlySerializedAs("_harvestCooldown")]
        private float _attackCooldown = 1.0f;

        public float AttackCooldown => _attackCooldown;

#region Animations
        [SerializeField]
        private string _idleAnimation = "beetle_idle";

        public string IdleAnimation => _idleAnimation;
#endregion
    }
}
