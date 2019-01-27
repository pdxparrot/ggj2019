using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.World
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

                // TODO hack for CheckOccupied
                int maxiters = SpawnPoints.Count * 2;
                int i = 0;

                SpawnPoint result = null;

                do {
                    switch(spawnType.SpawnMethod)
                    {
                    case SpawnData.SpawnMethod.RoundRobin:
                        SpawnPoint spawnPoint = SpawnPoints[_nextRoundRobinIndex];
                        AdvanceRoundRobin();
                        result = spawnPoint;
                        break;
                    case SpawnData.SpawnMethod.Random:
                        result = PartyParrotManager.Instance.Random.GetRandomEntry(SpawnPoints);
                        break;
                    default:
                        Debug.LogWarning($"Unsupported spawn method {spawnType.SpawnMethod}, using Random");
                        result = PartyParrotManager.Instance.Random.GetRandomEntry(SpawnPoints);
                        break;
                    }

                    if(spawnType.CheckOccupied && result.Occupied) {
                        result = null;
                    }

                    ++i;
                } while(result == null && i < maxiters);

                return result;
            }

            private void AdvanceRoundRobin()
            {
                _nextRoundRobinIndex = (_nextRoundRobinIndex + 1) % SpawnPoints.Count;
            }
        }

        [SerializeField]
        private SpawnData _spawnData;

        private readonly Dictionary<string, SpawnPointSet> _spawnPoints = new Dictionary<string, SpawnPointSet>();

#region Unity Lifecycle
        private void Awake()
        {
            _spawnData.Initialize();
        }
#endregion

#region Registration
        public virtual void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            Debug.Log($"Registering spawnpoint {spawnPoint.name} of type {spawnPoint.Tag}");

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

            var spawnPointType = _spawnData.GetType(tag);
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
