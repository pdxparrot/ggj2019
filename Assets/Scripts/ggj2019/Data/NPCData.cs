using System;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.State;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
    [CreateAssetMenu(fileName="NPCData", menuName="pdxpartyparrot/ggj2019/Data/NPC Data")]
    [Serializable]
    public sealed class NPCData : Game.Data.GameData
    {
        [Header("Flowers")]
        [SerializeField] public NPCFlower FlowerPrefab;
        [SerializeField] public int FlowerInitialPollen;
        [SerializeField] public int FlowerDrainRate;
        [Space(10)]

        [Header("Wasp")]
        [Space(10)]
        [SerializeField] public NPCWasp WaspPrefab;

        [Header("Beetle")]
        [Space(10)]
        [SerializeField] public NPCBeetle BeetlePrefab;
    }
}

