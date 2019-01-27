using System;
using System.Collections;
using System.Collections.Generic;
using pdxpartyparrot.Core;
using UnityEngine;

using pdxpartyparrot.Game.World;
using pdxpartyparrot.Core.Util;

public class NPCSpawner : MonoBehaviour
{
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
    };

    // Each wave of npc spawning
    [Serializable]
    public struct SpawnWave {
        [SerializeField] public List<SpawnGroup> SpawnGroups;
        [SerializeField] public float Duration;
    };

    [SerializeField]
    public NPCData[] NPCTypes = new NPCData[3 /*NPCType.Count*/];

    [SerializeField]
    public List<SpawnWave> Waves;

    // Runtime state
    private int _waveIndex;
    private Timer _waveTimer;
    private List<Timer> _spawnTimers;

    // The current wave
    public SpawnWave Wave {
        get {
            return Waves[_waveIndex];
        }
    }

    void Start() {
        _waveTimer = new Timer();
        _spawnTimers = new List<Timer>();
        FirstWave();
    }

    void Update() {
        _waveTimer.Update(Time.deltaTime);
        if(!_waveTimer.IsRunning) {
            NextWave();
        }

        for(int i = 0; i < _spawnTimers.Count; ++i) {
            _spawnTimers[i].Update(Time.deltaTime);
            if(!_spawnTimers[i].IsRunning) {
                Spawn(i);
            }
        }
    }

    // Advance Waves
    public void FirstWave() {
        _waveIndex = -1;
        NextWave();
    }
    public void NextWave() {
        if(_waveIndex + 1 < Waves.Count)
            ++_waveIndex;

        _waveTimer.Start(Wave.Duration);

        _spawnTimers.Clear();
        for(int i = 0; i < Wave.SpawnGroups.Count; ++i) {
            var grp = Wave.SpawnGroups[i];

            _spawnTimers.Add(new Timer());
            _spawnTimers[i].Start(PartyParrotManager.Instance.Random.NextSingle(grp.Delay.Min, grp.Delay.Max));
        }
    }

    // Spawning
    private void Spawn(int grpidx) {
        var grp = Wave.SpawnGroups[grpidx];
        var npc = NPCTypes[(int)grp.Type];

        int ct = PartyParrotManager.Instance.Random.Next(grp.Count.Min, grp.Count.Max);

        var spawnpt = SpawnManager.Instance.GetSpawnPoint(npc.Tag);
        if(spawnpt) {
            var actor = Instantiate(npc.Prefab);
            spawnpt.Spawn(actor);

            _spawnTimers[grpidx].Start(PartyParrotManager.Instance.Random.NextSingle(grp.Delay.Min, grp.Delay.Max));
        }
    }


}
