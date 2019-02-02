using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019;

using UnityEngine;
using UnityEngine.Assertions;

public class NPCBeetle : NPCEnemy
{
    [SerializeField]
    private float _harvestCooldown = 1.0f;

    [SerializeField]
    private NPCFlower _flower;

    public int Pollen { get; private set; }

    private readonly Timer _harvestCooldownTimer = new Timer();

    private SpawnPoint _spawnpoint;

#region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();

        Pool.Add(this);
    }

    protected override void OnDestroy()
    {
        Pool.Remove(this);

        if(_flower != null) {
            _flower.CanSpawnPollen = true;
        }

        base.OnDestroy();
    }

    private void Update()
    {
        if(IsDead) {
            return;
        }

        float dt = Time.deltaTime;

        _harvestCooldownTimer.Update(dt);
    }
#endregion

    public override void OnSpawn(SpawnPoint spawnpoint)
    {
        base.OnSpawn(spawnpoint);

        if(!spawnpoint.Acquire(this)) {
            Debug.LogError("Unable to acquire spawnpoint!");
            Destroy(gameObject);
            return;
        }
        _spawnpoint = spawnpoint;
        

        _flower = NPCFlower.Nearest(transform.position);
        if(null == _flower || !_flower.Collides(this) || _flower.IsDead) {
            Debug.LogWarning("Spawned on a dead / missing flower!");
            Destroy(gameObject);
            return;
        }

        _flower.CanSpawnPollen = false;

        //Assert.IsTrue(_flower.IsReady && _flower.CanSpawnPollen && _flower.HasPollen);

        _harvestCooldownTimer.Start(_harvestCooldown, HarvestFlower);
    }

    private void HarvestFlower()
    {
        if(GameManager.Instance.IsGameOver) {
            return;
        }

        Pollen += _flower.BeetleHarvest();
        if(_flower.IsDead) {
            _flower = null;

            Kill();
            return;
        }

        _harvestCooldownTimer.Start(_harvestCooldown, HarvestFlower);
    }

    public override void Kill()
    {
        _spawnpoint.Release();
        _spawnpoint = null;

        base.Kill();

        _harvestCooldownTimer.Stop();
        GameManager.Instance.BeetleKilled();
    }

    public static ProxPool<NPCBeetle> Pool = new ProxPool<NPCBeetle>();

    public static NPCBeetle Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist);
    }
}
