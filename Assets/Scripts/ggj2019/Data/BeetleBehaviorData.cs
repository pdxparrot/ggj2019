using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="BeetleBehaviorData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/BeetleBehavior Data")]
    [Serializable]
    public sealed class BeetleBehaviorData : EnemyBehaviorData
    {
        [Space(10)]

        [Header("Beetles")]

        [SerializeField]
        [FormerlySerializedAs("_harvestCooldown")]
        private float _attackCooldown = 1.0f;

        public float AttackCooldown => _attackCooldown;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _idleAnimation = "beetle_idle";

        public string IdleAnimation => _idleAnimation;
#endregion
    }
}
