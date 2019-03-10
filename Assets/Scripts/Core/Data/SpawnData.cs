using System;
using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot.Core.Data
{
    [CreateAssetMenu(fileName="SpawnData", menuName="pdxpartyparrot/Core/Data/Spawn Data")]
    [Serializable]
    public class SpawnData : ScriptableObject
    {
        public enum SpawnMethod
        {
            Random,
            RoundRobin
        }

// TODO use privates, not publics
        [Serializable]
        public struct SpawnPointType
        {
            public string Tag;

            public SpawnMethod SpawnMethod;
        }

        [SerializeField]
        private string _playerSpawnPointTag;

        public string PlayerSpawnPointTag => _playerSpawnPointTag;

        [SerializeField]
        private SpawnPointType[] _types;

        public IReadOnlyCollection<SpawnPointType> Types => _types;
    }
}
