using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCWaspData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Wasp Data")]
    [Serializable]
    public sealed class WaspData : EnemyData
    {
        [SerializeField]
        private float _speed = 1.0f;

        public float Speed => _speed;

        [SerializeField]
        private float _attackCooldown = 1.0f;

        public float AttackCooldown => _attackCooldown;

#region Animations
        [SerializeField]
        private string _idleAnimationName = "wasp_hover";

        public string IdleAnimationName => _idleAnimationName;

        [SerializeField]
        private string _flyingAnimationName = "wasp_hover";

        public string FlyingAnimationName => _flyingAnimationName;
#endregion
    }
}
