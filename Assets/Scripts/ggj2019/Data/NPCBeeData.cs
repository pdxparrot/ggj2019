using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCBeeData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Bee Data")]
    [Serializable]
    public sealed class NPCBeeData : NPCData
    {
        [SerializeField]
        private float _swarmSpeedModifier = 2.0f;

        public float SwarmSpeedModifier => _swarmSpeedModifier;

        [SerializeField]
        private FloatRange _offsetUpdateRange = new FloatRange(0.2f, 0.5f);

        public FloatRange OffsetUpdateRange => _offsetUpdateRange;

#region Animations
        [SerializeField]
        private string _hoverAnimationName = "bee_hover";

        public string HoverAnimationName => _hoverAnimationName;

        [SerializeField]
        private string _flightAnimationName = "bee-flight";

        public string FlightAnimationName => _flightAnimationName;
#endregion
    }
}
