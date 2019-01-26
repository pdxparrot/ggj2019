using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    // Tunable params

    public struct Range {
        public int Min;
        public int Max;
    };

    // Rules for spawning one type of npcs
    public struct SpawnGroup {
        public NPCBase NPC;

        public Range Delay;
        public Range Count;
    };

    // Each wave of npc spawning
    public struct SpawnWave {
        public List<SpawnGroup> SpawnGroups;
        public float Duration;
    };

    public List<SpawnWave> Waves;

    // Runtime state
    public int WaveIndex { get; private set; }
    public float WaveTime { get; private set; }
    public List<float> SpawnTimers { get; private set; }

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
        WaveTime += Time.deltaTime;
        if(WaveTime > Wave.Duration) {
            NextWave();
        }

        for(int i = 0; i < SpawnTimers.Count; ++i) {
            SpawnTimers[i] += Time.deltaTime;
            if(SpawnTimers[i] < 0.0f) {
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

        WaveTime = 0;
    }

    // Spawning
    private void Spawn(int grpidx) {
        var grp = Wave.SpawnGroups[grpidx];

        int ct = random.Next(grp.Count.Min, grp.Count.Max);
        Instantiate(grp.NPC);

        //SpawnTimers[grpidx] = random.NextDouble(grp.Delay);
    }


}
