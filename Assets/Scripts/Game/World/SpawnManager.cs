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
        [SerializeField]
        private SpawnData _spawnData;

        private readonly Dictionary<string, HashSet<SpawnPoint>> _spawnPoints = new Dictionary<string, HashSet<SpawnPoint>>();

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

            _spawnPoints.GetOrAdd(spawnPoint.Tag).Add(spawnPoint);
        }

        public virtual void UnregisterSpawnPoint(SpawnPoint spawnPoint)
        {
            //Debug.Log($"Unregistering spawnpoint {spawnPoint.name}");

            if(_spawnPoints.TryGetValue(spawnPoint.Tag, out var spawnPoints)) {
                spawnPoints.Remove(spawnPoint);
            }
        }
#endregion

        [CanBeNull]
        public SpawnPoint GetSpawnPoint(string tag)
        {
            if(!_spawnPoints.TryGetValue(tag, out var spawnPoints)) {
                Debug.LogWarning($"No spawn points with tag {tag} registered on spawn, are there any in the scene?");
                return null;
            }

            var spawnPointType = _spawnData.GetType(tag);
            switch(spawnPointType.SpawnMethod)
            {
            case SpawnData.SpawnMethod.RoundRobin:
                Debug.LogError("RoundRobin spawning should not be random");
                return PartyParrotManager.Instance.Random.GetRandomEntry(spawnPoints);
            case SpawnData.SpawnMethod.Random:
            default:
                return PartyParrotManager.Instance.Random.GetRandomEntry(spawnPoints);
            }
        }

        [CanBeNull]
        public SpawnPoint GetPlayerSpawnPoint(int controllerId)
        {
            // TODO: can the controllerId factor into this?
            return GetSpawnPoint(_spawnData.PlayerSpawnPointTag);
        }
    }
}
