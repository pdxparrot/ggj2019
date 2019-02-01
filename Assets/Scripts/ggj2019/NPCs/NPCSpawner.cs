using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;

// TODO: this is a duration spawner, we could also have a spawner that always spawns a full wave before the next one (no duration)
public class NPCSpawner : SingletonBehavior<NPCSpawner>
{
    public event Action WaveStartEvent;

    [SerializeField]
    private WaveSpawnData _waveSpawnData;

    [SerializeField]
    [ReadOnly]
    private int _waveIndex;

    private readonly Timer _waveTimer = new Timer();

#region Unity Lifecycle
    private void Update()
    {
        float dt = Time.deltaTime;

        _waveTimer.Update(dt);
        _waveSpawnData.UpdateWave(_waveIndex, dt);
    }
#endregion

    public void Initialize()
    {
        _waveSpawnData.Initialize();

        _waveIndex = -1;

        Run();
    }

    public void Shutdown()
    {
        _waveSpawnData.StopWave(_waveIndex);

        _waveTimer.Stop();

        _waveSpawnData.Shutdown();
    }

    private void Run()
    {
        // stop the current wave timers
        if(_waveIndex >= 0) {
            _waveSpawnData.StopWave(_waveIndex);
        }

        // advance the wave (end game if we hit the end)
        _waveIndex++;
        if(_waveIndex >= _waveSpawnData.WaveCount) {
            GameManager.Instance.EndGame();
            return;
        }

        // start the next wave of timers
        _waveSpawnData.StartWave(_waveIndex);
        _waveTimer.Start(_waveSpawnData.WaveDuration(_waveIndex), Run);

        WaveStartEvent?.Invoke();
    }
}
