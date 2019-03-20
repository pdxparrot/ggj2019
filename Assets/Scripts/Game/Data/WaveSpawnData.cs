using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="WaveSpawnData", menuName="pdxpartyparrot/Game/Data/Wave Spawn Data")]
    [Serializable]
    public class WaveSpawnData : ScriptableObject
    {
// TODO: this is really hard to work with when duplicating data
// might be easier to split the SpawnGroupData into its own data file

// TODO: SpawnGroupData
        [Serializable]
        public class SpawnGroup
        {
            [SerializeField]
            private Actor _actorPrefab;

            public Actor ActorPrefab => _actorPrefab;

            [SerializeField]
            [FormerlySerializedAs("npcData")]
            private NPCBehaviorData _npcBehaviorData;

            public NPCBehaviorData NPCBehaviorData => _npcBehaviorData;

            [SerializeField]
            [Tooltip("The spawnpoint tag")]
            private string _tag;
   
            public string Tag => _tag;

            [Space(10)]

            [SerializeField]
            [Tooltip("Time between spawns, in seconds")]
            private Range _delay;

            public Range Delay => _delay;

            [SerializeField]
            [Tooltip("How many actors to spawn each time we spawn")]
            private Range _count;

            public Range Count => _count;

            [Space(10)]

            [SerializeField]
            [Tooltip("How many objects to pre-allocate in the object pool (for pooled objects)")]
            private int _poolSize = 1;

            public int PoolSize => _poolSize;

            [Space(10)]

            [SerializeField]
            [Tooltip("Should we only spawn the wave once?")]
            private bool _once;

            public bool Once => _once;
        }

// TODO: SpawnWaveData
        [Serializable]
        public class SpawnWave
        {
            [SerializeField]
            [Tooltip("The duration of the wave")]
            private float _duration;

            public float Duration => _duration;

            [SerializeField]
            private SpawnGroup[] _spawnGroups;

            public IReadOnlyCollection<SpawnGroup> SpawnGroups => _spawnGroups;

            [SerializeField]
            private AudioClip _waveMusic;

            public AudioClip WaveMusic => _waveMusic;
        }

        [SerializeField]
        private SpawnWave[] _waves;

        [SerializeField]
        private float _musicTransitionSeconds = 1.0f;

        public float MusicTransitionSeconds => _musicTransitionSeconds;

        public IReadOnlyCollection<SpawnWave> Waves => _waves;
    }
}
