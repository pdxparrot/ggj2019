using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="CollectableData", menuName="pdxpartyparrot/ggj2019/Data/Collectable Data")]
    [Serializable]
    public sealed class CollectableData : ScriptableObject
    {
#region Pollen
        [Header("Pollen")]

        [SerializeField]
        private float _sideDistance = 1.0f;

        public float SideDistance => _sideDistance;

        [SerializeField]
        private float _sideSpeed = 1.0f;

        public float SideSpeed => _sideSpeed;

        [SerializeField]
        private float _upwardSpeed = 1.0f;

        public float UpwardSpeed => _upwardSpeed;

        [SerializeField]
        private float _goToHiveSpeed = 1.0f;

        public float GoToHiveSpeed => _goToHiveSpeed;

        [SerializeField]
        private float _followPlayerSpeed = 1.0f;

        public float FollowPlayerSpeed => _followPlayerSpeed;
#endregion
    }
}
