using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.NPCs;

using UnityEngine;

namespace pdxpartyparrot.Game.World
{
    public class WaveSpawner : MonoBehaviour
    {
#region Events
        public event EventHandler<SpawnWaveEventArgs> WaveStartEvent;
        public event EventHandler<SpawnWaveEventArgs> WaveCompleteEvent;
#endregion

        [Serializable]
        protected class SpawnWave
        {
            [Serializable]
            protected class SpawnGroup
            {
                private readonly WaveSpawnData.SpawnGroup _spawnGroupData;

                [SerializeField]
                [ReadOnly]
                private /*readonly*/ Timer _spawnTimer = new Timer();

                private string PoolTag => $"spawnGroup_{_spawnGroupData.Tag}";

                private GameObject _poolContainer;

                private readonly  WaveSpawner _owner;

                public SpawnGroup(WaveSpawnData.SpawnGroup spawnGroupData, WaveSpawner owner)
                {
                    _spawnGroupData = spawnGroupData;
                    _owner = owner;
                }

                public void Initialize(float waveDuration)
                {
                    PooledObject pooledObject = _spawnGroupData.ActorPrefab.GetComponent<PooledObject>();
                    if(null != pooledObject) {
                        _poolContainer = new GameObject(PoolTag);
                        _poolContainer.transform.SetParent(_owner.transform);

                        int count = Mathf.Max(_spawnGroupData.PoolSize, 1);
                        if(ObjectPoolManager.Instance.HasPool(PoolTag)) {
                            ObjectPoolManager.Instance.EnsurePoolSize(PoolTag, count);
                        } else {
                            ObjectPoolManager.Instance.InitializePoolAsync(PoolTag, pooledObject, count);
                        }
                    }
                }

                public void Shutdown()
                {
                    if(null != _poolContainer) {
                        if(ObjectPoolManager.HasInstance) {
                            ObjectPoolManager.Instance.DestroyPool(PoolTag);
                        }

                        Destroy(_poolContainer);
                        _poolContainer = null;
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
                    int amount = _spawnGroupData.Count.GetRandomValue();
                    for(int i=0; i<amount; ++i) {
                        var spawnPoint = SpawnManager.Instance.GetSpawnPoint(_spawnGroupData.Tag);
                        if(null == spawnPoint) {
                            //Debug.LogWarning($"No spawnpoints for {_spawnGroupData.Tag}!");
                        } else {
                            Actor actor = null;
                            if(null != _poolContainer) {
                                actor = ObjectPoolManager.Instance.GetPooledObject<Actor>(PoolTag);
                                if(null == actor) {
                                    Debug.LogWarning($"Actor for pool {PoolTag} missing its PooledObject!");
                                    continue;
                                }

                                if(!spawnPoint.Spawn(actor, Guid.NewGuid())) {
                                    actor.GetComponent<PooledObject>().Recycle();
                                    Debug.LogWarning($"Failed to spawn actor for {_spawnGroupData.Tag}");
                                    continue;
                                }
                            } else {
                                actor = spawnPoint.SpawnPrefab(_spawnGroupData.ActorPrefab, Guid.NewGuid());
                                if(null == actor) {
                                    continue;
                                }
                            }
                            actor.transform.SetParent(_poolContainer.transform);

                            // init the NPC with data if we have it
                            if(null != _spawnGroupData.NPCData) {
                                INPC npc = actor.GetComponent<INPC>();
                                if(null != npc) {
                                    npc.Initialize(_spawnGroupData.NPCData);
                                }
                            }
                        }

                        if(!_spawnGroupData.Once) {
                            _spawnTimer.Start(_spawnGroupData.Delay, Spawn);
                        }
                    }
                }
            }

            private readonly WaveSpawnData.SpawnWave _spawnWaveData;

            public float Duration => _spawnWaveData.Duration;

            [SerializeField]
            [ReadOnly]
            private /*readonly*/ List<SpawnGroup> _spawnGroups = new List<SpawnGroup>();

            private readonly WaveSpawner _owner;

            public SpawnWave(WaveSpawnData.SpawnWave spawnWaveData, WaveSpawner owner)
            {
                _spawnWaveData = spawnWaveData;
                _owner = owner;

                foreach(WaveSpawnData.SpawnGroup spawnGroup in _spawnWaveData.SpawnGroups) {
                    _spawnGroups.Add(new SpawnGroup(spawnGroup, _owner));
                }
            }

            public void Initialize()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Initialize(Duration);
                }
            }

            public void Shutdown()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Shutdown();
                }
            }

            public void Start()
            {
                foreach(SpawnGroup spawnGroup in _spawnGroups) {
                    spawnGroup.Start();
                }
                AudioManager.Instance.TransitionMusicAsync(_spawnWaveData.WaveMusic, _owner.WaveSpawnData.MusicTransitionSeconds);
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

        public WaveSpawnData WaveSpawnData => _waveSpawnData;

        [SerializeField]
        [ReadOnly]
        private int _currentWaveIndex;

        public int CurrentWaveIndex => _currentWaveIndex;

        private bool HasCurrentWave => _currentWaveIndex >= 0 && _currentWaveIndex < _spawnWaves.Count;

        [CanBeNull]
        private SpawnWave CurrentWave => HasCurrentWave ? _spawnWaves[_currentWaveIndex] : null;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _waveTimer = new Timer();

        private readonly List<SpawnWave> _spawnWaves = new List<SpawnWave>();

#region Unity Lifecycle
        private void Awake()
        {
            foreach(WaveSpawnData.SpawnWave spawnWave in _waveSpawnData.Waves) {
                _spawnWaves.Add(new SpawnWave(spawnWave, this));
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
