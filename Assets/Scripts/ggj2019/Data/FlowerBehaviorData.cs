using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="FlowerBehaviorData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/FlowerBehavior Data")]
    [Serializable]
    public sealed class FlowerBehaviorData : NPCBehaviorData
    {
        [Space(10)]

        [Header("Pollen")]

        [SerializeField]
        private int _pollen = 5;

        public int Pollen => _pollen;

        [SerializeField]
        private FloatRange _pollenSpawnCooldown = new FloatRange(1.0f, 5.0f);

        public FloatRange PollenSpawnCooldown => _pollenSpawnCooldown;

        [SerializeField]
        private PollenBehaviorData _pollenBehaviorData;

        public PollenBehaviorData PollenBehaviorData => _pollenBehaviorData;

        [SerializeField]
        private int _maxPollen = 2;

        public int MaxPollen => _maxPollen;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _idleAnimationName = "flower_idle";

        public string IdleAnimationName => _idleAnimationName;
#endregion
    }
}
