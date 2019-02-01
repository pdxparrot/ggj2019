using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Data
{
// TODO: move the state out of this and back into the spawner

    [CreateAssetMenu(fileName="WaveSpawnData", menuName="pdxpartyparrot/Game/Data/Wave Spawn Data")]
    [Serializable]
    public class WaveSpawnData : ScriptableObject
    {
        [Serializable]
        public class ActorData
        {
            [SerializeField]
            private Actor _prefab;

            public Actor Prefab => _prefab;

            [SerializeField]
            private string _tag;

            public string Tag => _tag;

            [SerializeField]
            [ReadOnly]
            private bool _pooled;
    
            public bool Pooled
            {
                get => _pooled;
                set => _pooled = value;
            }
        }

        [Serializable]
        public class SpawnGroup
        {
            [SerializeField]
            private string _tag;

            [SerializeField]
            private Range _delay;

            [SerializeField]
            private Range _count;

            [SerializeField]
            private bool _once;

            [SerializeField]
            [ReadOnly]
            private ActorData _actorData;

            private readonly Timer _spawnTimer = new Timer();

            public void Start(IReadOnlyCollection<ActorData> actors)
            {
                _actorData = null;
                foreach(ActorData actorData in actors) {
                    if(actorData.Tag == _tag) {
                        _actorData = actorData;
                        break;
                    }
                }
                _spawnTimer.Start(_delay, Spawn);
            }

            public void Update(float dt)
            {
                _spawnTimer.Update(dt);
            }

            public void Stop()
            {
                _spawnTimer.Stop();
            }

            private void Spawn()
            {
                if(null == _actorData) {
                    Debug.LogWarning($"Cannot spawn invalid prefab {_tag}");
                    return;
                }

                int amount = _count.GetValue();
                for(int i=0; i<amount; ++i) {
                    var spawnPoint = SpawnManager.Instance.GetSpawnPoint(_actorData.Tag);
                    if(null == spawnPoint) {
                        continue;
                    }
                    spawnPoint.SpawnPrefab(_actorData.Prefab);

                    if(!_once) {
                        _spawnTimer.Start(_delay, Spawn);
                    }
                }
            }
        }

        [Serializable]
        public class SpawnWave
        {
            [SerializeField]
            private float _duration;

            public float Duration => _duration;

            [SerializeField]
            private SpawnGroup[] _spawnGroups;

            public IReadOnlyCollection<SpawnGroup> SpawnGroups => _spawnGroups;

            public void Start(IReadOnlyCollection<ActorData> actors)
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Start(actors);
                }
            }

            public void Stop()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Stop();
                }
            }

            public void Update(float dt)
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Update(dt);
                }
            }
        };

        [SerializeField]
        private ActorData[] _actors;

        [SerializeField]
        private SpawnWave[] _waves;

        public int WaveCount => _waves.Length;

        public void Initialize()
        {
            foreach(ActorData actorData in _actors) {
                actorData.Pooled = false;

                // TODO: count should come from the max wave size or something
                PooledObject pooledObject = actorData.Prefab.GetComponent<PooledObject>();
                if(null != pooledObject) {
                    ObjectPoolManager.Instance.InitializePool(actorData.Tag, pooledObject, 10);
                    actorData.Pooled = true;
                }
            }
        }

        public void Shutdown()
        {
            foreach(ActorData actorData in _actors) {
                if(actorData.Pooled) {
                    ObjectPoolManager.Instance.DestroyPool(actorData.Tag);
                }
                actorData.Pooled = false;
            }
        }

        public float WaveDuration(int waveIndex)
        {
            if(waveIndex < 0 || waveIndex >= _waves.Length) {
                return 0;
            }
            return _waves[waveIndex].Duration;
        }

        public void StartWave(int waveIndex)
        {
            if(waveIndex < 0 || waveIndex >= _waves.Length) {
                return;
            }
            _waves[waveIndex].Start(_actors);
        }

        public void UpdateWave(int waveIndex, float dt)
        {
            if(waveIndex < 0 || waveIndex >= _waves.Length) {
                return;
            }
            _waves[waveIndex].Update(dt);
        }

        public void StopWave(int waveIndex)
        {
            if(waveIndex < 0 || waveIndex >= _waves.Length) {
                return;
            }
            _waves[waveIndex].Stop();
        }
    }
}
