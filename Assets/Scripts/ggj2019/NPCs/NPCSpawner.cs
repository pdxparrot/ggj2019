using System;
using System.Collections;
using System.Collections.Generic;
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
    public int WaveIndex { get; private set; }
    public Timer WaveTimer { get; private set; }
    public List<Timer> SpawnTimers { get; private set; }

    private readonly System.Random random = new System.Random();

    // The current wave
    public SpawnWave Wave {
        get {
            return Waves[WaveIndex];
        }
    }

    void Start() {
        FirstWave();
    }

    void Update() {
        WaveTimer.Update(Time.deltaTime);
        if(!WaveTimer.IsRunning) {
            NextWave();
        }

        for(int i = 0; i < SpawnTimers.Count; ++i) {
            SpawnTimers[i].Update(Time.deltaTime);
            if(!SpawnTimers[i].IsRunning) {
                Spawn(i);
            }
        }
    }

    // Advance Waves
    public void FirstWave() {
        WaveIndex = -1;
        NextWave();
    }
    public void NextWave() {
        if(WaveIndex + 1 < Waves.Count)
            ++WaveIndex;

        WaveTimer.Start(Wave.Duration);

        SpawnTimers.Clear();
        for(int i = 0; i < Wave.SpawnGroups.Count; ++i) {
            var grp = Wave.SpawnGroups[i];

            SpawnTimers.Add(new Timer());
            SpawnTimers[i].Start(random.NextSingle(grp.Delay.Min, grp.Delay.Max));
        }
    }

    // Spawning
    private void Spawn(int grpidx) {
        var grp = Wave.SpawnGroups[grpidx];
        var npc = NPCTypes[(int)grp.Type];

        int ct = random.Next(grp.Count.Min, grp.Count.Max);

        var spawnpt = SpawnManager.Instance.GetSpawnPoint(npc.Tag);
        if(spawnpt) {
            spawnpt.Spawn(npc.Prefab);
        }

        SpawnTimers[grpidx].Start(random.NextSingle(grp.Delay.Min, grp.Delay.Max));
    }


}
