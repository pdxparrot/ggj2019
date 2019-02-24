using System;

using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="PollenData", menuName="pdxpartyparrot/ggj2019/Data/Collectables/Pollen Data")]
    [Serializable]
    public sealed class PollenData : CollectableData
    {
        [SerializeField]
        [FormerlySerializedAs("_upwardSpeed")]
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
