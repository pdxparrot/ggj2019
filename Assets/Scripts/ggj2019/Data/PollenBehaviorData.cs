using System;

using pdxpartyparrot.Core.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="PollenBehaviorData", menuName="pdxpartyparrot/ggj2019/Data/Collectables/PollenBehavior Data")]
    [Serializable]
    public sealed class PollenBehaviorData : ActorBehaviorData
    {
        [Space(10)]

        [Header("Pollen")]

        [SerializeField]
        private float _floatSpeed = 1.0f;

        public float FloatSpeed => _floatSpeed;

        [SerializeField]
        private float _goToHiveSpeed = 1.0f;

        public float GoToHiveSpeed => _goToHiveSpeed;

        [SerializeField]
        private float _followPlayerSpeed = 1.0f;

        public float FollowPlayerSpeed => _followPlayerSpeed;
    }
}
