using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="BeeData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Bee Data")]
    [Serializable]
    public sealed class BeeData : NPCBehaviorData
    {
        [Space(10)]

        [Header("Bees")]

        [SerializeField]
        private float _swarmSpeedModifier = 2.0f;

        public float SwarmSpeedModifier => _swarmSpeedModifier;

        public float SwarmSpeed => MoveSpeed * SwarmSpeedModifier;

        [SerializeField]
        private FloatRange _offsetUpdateRange = new FloatRange(0.2f, 0.5f);

        public FloatRange OffsetUpdateRange => _offsetUpdateRange;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _idleAnimationName = "bee_hover";

        public string IdleAnimationName => _idleAnimationName;

        [SerializeField]
        private string _flyingAnimationName = "bee-flight";

        public string FlyingAnimationName => _flyingAnimationName;
#endregion
    }
}
