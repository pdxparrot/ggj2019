﻿using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.World
{
    public class SpawnManager : SingletonBehavior<SpawnManager>
    {
        private sealed class SpawnPointSet
        {
            public List<SpawnPoint> SpawnPoints { get; } = new List<SpawnPoint>();

            private int _nextRoundRobinIndex;

            public void SeedRoundRobin()
            {
                _nextRoundRobinIndex = PartyParrotManager.Instance.Random.Next(SpawnPoints.Count);
            }

            public SpawnPoint GetSpawnPoint(SpawnData.SpawnPointType spawnType)
            {
                if(SpawnPoints.Count < 1) {
                    return null;
                }

                // just in case
                if(_nextRoundRobinIndex >= SpawnPoints.Count) {
                    AdvanceRoundRobin();
                }

                switch(spawnType.SpawnMethod)
                {
                case SpawnData.SpawnMethod.RoundRobin:
                    SpawnPoint spawnPoint = SpawnPoints[_nextRoundRobinIndex];
                    AdvanceRoundRobin();
                    return spawnPoint;
                case SpawnData.SpawnMethod.Random:
                    return PartyParrotManager.Instance.Random.GetRandomEntry(SpawnPoints);
                default:
                    Debug.LogWarning($"Unsupported spawn method {spawnType.SpawnMethod}, using Random");
                    return PartyParrotManager.Instance.Random.GetRandomEntry(SpawnPoints);
                }
            }

            private void AdvanceRoundRobin()
            {
                _nextRoundRobinIndex = (_nextRoundRobinIndex + 1) % SpawnPoints.Count;
            }
        }

        [SerializeField]
        private SpawnData _spawnData;

        private readonly Dictionary<string, SpawnData.SpawnPointType> _spawnTypes = new Dictionary<string, SpawnData.SpawnPointType>();

        private readonly Dictionary<string, SpawnPointSet> _spawnPoints = new Dictionary<string, SpawnPointSet>();

#region Unity Lifecycle
        private void Awake()
        {
            foreach(var spawnPointType in _spawnData.Types) {
                if(_spawnTypes.ContainsKey(spawnPointType.Tag)) {
                    Debug.LogError($"Duplicate spawn point tag {spawnPointType.Tag}, ignoring");
                    continue;
                }
                _spawnTypes.Add(spawnPointType.Tag, spawnPointType);
            }
        }
#endregion

#region Registration
        public virtual void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            //Debug.Log($"Registering spawnpoint {spawnPoint.name} of type {spawnPoint.Tag}");

            _spawnPoints.GetOrAdd(spawnPoint.Tag).SpawnPoints.Add(spawnPoint);
        }

        public virtual void UnregisterSpawnPoint(SpawnPoint spawnPoint)
        {
            //Debug.Log($"Unregistering spawnpoint {spawnPoint.name}");

            if(_spawnPoints.TryGetValue(spawnPoint.Tag, out var spawnPoints)) {
                spawnPoints.SpawnPoints.Remove(spawnPoint);
            }
        }
#endregion

        public void Initialize()
        {
            Debug.Log("Seeding spawn points...");
            foreach(var kvp in _spawnPoints) {
                kvp.Value.SeedRoundRobin();
            }
        }

        [CanBeNull]
        public SpawnPoint GetSpawnPoint(string tag)
        {
            if(!_spawnPoints.TryGetValue(tag, out var spawnPoints)) {
                Debug.LogWarning($"No spawn points with tag {tag} registered on spawn, are there any in the scene?");
                return null;
            }

            var spawnPointType = _spawnTypes.GetOrDefault(tag);
            return spawnPoints.GetSpawnPoint(spawnPointType);
        }

        [CanBeNull]
        public SpawnPoint GetPlayerSpawnPoint(int controllerId)
        {
            // TODO: can the controllerId factor into this?
            return GetSpawnPoint(_spawnData.PlayerSpawnPointTag);
        }
    }
}
