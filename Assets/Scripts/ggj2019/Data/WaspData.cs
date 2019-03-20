using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCWaspData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Wasp Data")]
    [Serializable]
    public sealed class WaspData : EnemyData
    {
        [Space(10)]

        [Header("Wasps")]

        [SerializeField]
        private float _attackCooldown = 1.0f;

        public float AttackCooldown => _attackCooldown;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _idleAnimationName = "wasp_hover";

        public string IdleAnimationName => _idleAnimationName;

        [SerializeField]
        private string _flyingAnimationName = "wasp_hover";

        public string FlyingAnimationName => _flyingAnimationName;
#endregion
    }
}
