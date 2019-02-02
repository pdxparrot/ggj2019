using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class WaveSpawner : MonoBehaviour
    {
#region Events
        public event EventHandler<SpawnWaveEventArgs> WaveStartEvent;
        public event EventHandler<SpawnWaveEventArgs> WaveCompleteEvent;
#endregion

        protected class SpawnWave
        {
            protected class SpawnGroup
            {
                private readonly WaveSpawnData.SpawnGroup _spawnGroupData;

                private readonly Timer _spawnTimer = new Timer();

                private string PoolTag => $"spawnGroup_{_spawnGroupData.Tag}";

                public SpawnGroup(WaveSpawnData.SpawnGroup spawnGroupData)
                {
                    _spawnGroupData = spawnGroupData;
                }

                public void Initialize()
                {
                    PooledObject pooledObject = _spawnGroupData.ActorPrefab.GetComponent<PooledObject>();
                    if(null != pooledObject) {
                        if(ObjectPoolManager.Instance.HasPool(PoolTag)) {
                            ObjectPoolManager.Instance.EnsurePoolSize(PoolTag, _spawnGroupData.Count.Max);
                        } else {
                            ObjectPoolManager.Instance.InitializePool(PoolTag, pooledObject, _spawnGroupData.Count.Max);
                        }
                    }
                }

                public void Shutdown()
                {
                    PooledObject pooledObject = _spawnGroupData.ActorPrefab.GetComponent<PooledObject>();
                    if(null != pooledObject) {
                        ObjectPoolManager.Instance.DestroyPool(PoolTag);
                    }
                }

                public void Start()
                {
                    _spawnTimer.Start(_spawnGroupData.Delay, Spawn);
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
                    int amount = _spawnGroupData.Count.GetValue();
                    for(int i=0; i<amount; ++i) {
                        var spawnPoint = SpawnManager.Instance.GetSpawnPoint(_spawnGroupData.Tag);
                        if(null == spawnPoint) {
                            continue;
                        }

                        // TODO
                        /*Actor actor = ObjectPoolManager.Instance.GetPooledObject<Actor>(PoolTag);
                        spawnPoint.Spawn(actor);*/
                        spawnPoint.SpawnPrefab(_spawnGroupData.ActorPrefab);

                        if(!_spawnGroupData.Once) {
                            _spawnTimer.Start(_spawnGroupData.Delay, Spawn);
                        }
                    }
                }
            }

            private readonly WaveSpawnData.SpawnWave _spawnWaveData;

            public float Duration => _spawnWaveData.Duration;

            private readonly List<SpawnGroup> _spawnGroups = new List<SpawnGroup>();

            public SpawnWave(WaveSpawnData.SpawnWave spawnWaveData)
            {
                _spawnWaveData = spawnWaveData;

                foreach(WaveSpawnData.SpawnGroup spawnGroup in _spawnWaveData.SpawnGroups) {
                    _spawnGroups.Add(new SpawnGroup(spawnGroup));
                }
            }

            public void Initialize()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Initialize();
                }
            }

            public void Shutdown()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Shutdown();
                }
                _spawnGroups.Clear();
            }

            public void Start()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Start();
                }
            }

            public void Update(float dt)
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Update(dt);
                }
            }

            public void Stop()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Stop();
                }
            }
        }

        [SerializeField]
        private WaveSpawnData _waveSpawnData;

        [SerializeField]
        [ReadOnly]
        private int _currentWaveIndex;

        public int CurrentWaveIndex => _currentWaveIndex;

        private bool HasCurrentWave => _currentWaveIndex >= 0 && _currentWaveIndex < _spawnWaves.Count;

        private readonly List<SpawnWave> _spawnWaves = new List<SpawnWave>();

        [CanBeNull]
        private SpawnWave CurrentWave => HasCurrentWave ? _spawnWaves[_currentWaveIndex] : null;

        private readonly Timer _waveTimer = new Timer();

#region Unity Lifecycle
        private void Awake()
        {
            foreach(WaveSpawnData.SpawnWave spawnWave in _waveSpawnData.Waves) {
                _spawnWaves.Add(new SpawnWave(spawnWave));
            }
        }

        private void OnDestroy()
        {
            Shutdown();

            _spawnWaves.Clear();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _waveTimer.Update(dt);
            if(HasCurrentWave) {
                CurrentWave.Update(dt);
            }
        }
#endregion

        public void Initialize()
        {
            _currentWaveIndex = -1;

            foreach(SpawnWave spawnWave in _spawnWaves) {
                spawnWave.Initialize();
            }
        }

        public void Shutdown()
        {
            StopSpawner();

            foreach(SpawnWave spawnWave in _spawnWaves) {
                spawnWave.Shutdown();
            }
        }

        public void StartSpawner()
        {
            Run();
        }

        public void StopSpawner()
        {
            if(HasCurrentWave) {
                CurrentWave.Stop();
            }
            _waveTimer.Stop();
        }

        private void Run()
        {
            if(_currentWaveIndex >= _spawnWaves.Count) {
                return;
            }

            // stop the current wave timers
            if(_currentWaveIndex >= 0) {
                CurrentWave.Stop();
                WaveCompleteEvent?.Invoke(this, new SpawnWaveEventArgs(_currentWaveIndex, _currentWaveIndex >= _spawnWaves.Count - 1));
            }

            // advance the wave
            _currentWaveIndex++;
            if(_currentWaveIndex >= _spawnWaves.Count) {
                return;
            }

            // start the next wave of timers
            CurrentWave.Start();
            _waveTimer.Start(CurrentWave.Duration, Run);

            WaveStartEvent?.Invoke(this, new SpawnWaveEventArgs(_currentWaveIndex, _currentWaveIndex >= _spawnWaves.Count - 1));
        }
    }
}
