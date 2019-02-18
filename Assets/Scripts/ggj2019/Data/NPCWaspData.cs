using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCWaspData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Wasp Data")]
    [Serializable]
    public sealed class NPCWaspData : NPCEnemyData
    {
        [SerializeField]
        private float _speed = 1.0f;

        public float Speed => _speed;

#region Animations
        [SerializeField]
        private string _idleAnimationName = "wasp_hover";

        public string IdleAnimationName => _idleAnimationName;

        [SerializeField]
        private string _flyingAnimationName = "wasp_hover";

        public string FlyingAnimationName => _flyingAnimationName;

        [SerializeField]
        private string _attackAnimationName = "wasp_attack";

        public string AttackAnimationName => _attackAnimationName;
#endregion
    }
}
