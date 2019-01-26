using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="SpawnData", menuName="pdxpartyparrot/Game/Data/Spawn Data")]
    [Serializable]
    public sealed class SpawnData : ScriptableObject
    {
        public enum SpawnMethod
        {
            Random,
            RoundRobin
        }

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

        private readonly Dictionary<string, SpawnPointType> _spawnTypes = new Dictionary<string, SpawnPointType>();

        public void Initialize()
        {
            foreach(var spawnPointType in _types) {
                if(_spawnTypes.ContainsKey(spawnPointType.Tag)) {
                    Debug.LogError($"Duplicate spawn point tag {spawnPointType.Tag}, ignoring");
                }
                _spawnTypes.Add(spawnPointType.Tag, spawnPointType);
            }
        }

        public SpawnPointType GetType(string tag)
        {
            return _spawnTypes.GetOrDefault(tag);
        }
    }
}
