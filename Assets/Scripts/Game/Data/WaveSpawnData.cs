using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="WaveSpawnData", menuName="pdxpartyparrot/Game/Data/Wave Spawn Data")]
    [Serializable]
    public class WaveSpawnData : ScriptableObject
    {
// TODO: SpawnGroupData
        [Serializable]
        public class SpawnGroup
        {
            [SerializeField]
            private Actor _actorPrefab;

            public Actor ActorPrefab => _actorPrefab;

            [SerializeField]
            private string _tag;
   
            public string Tag => _tag;

            [SerializeField]
            private Range _delay;

            public Range Delay => _delay;

            [SerializeField]
            private Range _count;

            public Range Count => _count;

            [SerializeField]
            private bool _once;

            public bool Once => _once;
        }

// TODO: SpawnWaveData
        [Serializable]
        public class SpawnWave
        {
            [SerializeField]
            private float _duration;

            public float Duration => _duration;

            [SerializeField]
            private SpawnGroup[] _spawnGroups;

            public IReadOnlyCollection<SpawnGroup> SpawnGroups => _spawnGroups;
        }

        [SerializeField]
        private SpawnWave[] _waves;

        public IReadOnlyCollection<SpawnWave> Waves => _waves;
    }
}
