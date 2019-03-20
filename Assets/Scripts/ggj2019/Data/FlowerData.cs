﻿using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCFlowerData", menuName="pdxpartyparrot/ggj2019/Data/NPCs/Flower Data")]
    [Serializable]
    public sealed class FlowerData : NPCBehaviorData
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
        private PollenData _pollenData;

        public PollenData PollenData => _pollenData;

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
