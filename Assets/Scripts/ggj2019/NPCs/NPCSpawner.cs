using System;
using System.Collections.Generic;

using pdxpartyparrot.Core;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019;

using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public static NPCSpawner Instance { get; private set; }

    public event Action WaveStartEvent;

    // Tunable params

    public enum NPCType {
        Wasp,
        Beetle,
        Flower,
    };

    [Serializable]
    public struct NPCData {
        [SerializeField] public NPCBase Prefab;
        [SerializeField] public string Tag;
    }

    [Serializable]
    public struct Range {
        [SerializeField] public int Min;
        [SerializeField] public int Max;
    };

    // Rules for spawning one type of npcs
    [Serializable]
    public struct SpawnGroup {
        [SerializeField] public NPCType Type;
        [SerializeField] public Range Delay;
        [SerializeField] public Range Count;
        [SerializeField] public bool Once;
    };

    // Each wave of npc spawning
    [Serializable]
    public struct SpawnWave {
        [SerializeField] public SpawnGroup[] SpawnGroups;
        [SerializeField] public float Duration;
    };

    [SerializeField]
    public NPCData[] NPCTypes = new NPCData[3 /*NPCType.Count*/];

    [SerializeField]
    public SpawnWave[] Waves;

    // Runtime state
    private int _waveIndex;
    private readonly Timer _waveTimer = new Timer();
    private readonly List<Timer> _spawnTimers = new List<Timer>();

    // The current wave
    public SpawnWave Wave => Waves[_waveIndex];

    private bool _initialized;

#region Unity Lifecycle
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(!_initialized) {
            return;
        }

        float dt = Time.deltaTime;

        _waveTimer.Update(dt);
        foreach(Timer timer in _spawnTimers) {
            timer.Update(dt);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
#endregion

    public void Initialize()
    {
        FirstWave();

        _initialized = true;
    }

    // Advance Waves
    public void FirstWave()
    {
        _waveIndex = -1;

        NextWave();
    }

    public void NextWave()
    {
        if(_waveIndex + 1 < Waves.Length)
            ++_waveIndex;

        _waveTimer.Start(Wave.Duration, NextWave);
        WaveStartEvent?.Invoke();

        _spawnTimers.Clear();
        for(int i = 0; i < Wave.SpawnGroups.Length; ++i) {
            var grp = Wave.SpawnGroups[i];

            _spawnTimers.Add(new Timer());

            int idx = i;
            _spawnTimers[i].Start(PartyParrotManager.Instance.Random.NextSingle(grp.Delay.Min, grp.Delay.Max), () => {
                Spawn(idx);
            });
        }
    }

    // Spawning
    private void Spawn(int grpidx)
    {
        if(GameManager.Instance.IsGameOver) {
            return;
        }

        var grp = Wave.SpawnGroups[grpidx];
        var npc = NPCTypes[(int)grp.Type];

        int ct = PartyParrotManager.Instance.Random.Next(grp.Count.Min, grp.Count.Max);
        for(int i = 0; i < ct; ++i) {
            var spawnpt = SpawnManager.Instance.GetSpawnPoint(npc.Tag);
            if(spawnpt) {
                spawnpt.SpawnPrefab(npc.Prefab);
            }

            if(!grp.Once) {
                _spawnTimers[grpidx].Start(PartyParrotManager.Instance.Random.NextSingle(grp.Delay.Min, grp.Delay.Max), () => {
                    Spawn(grpidx);
                });
            }
        }
    }
}
