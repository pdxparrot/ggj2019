using pdxpartyparrot.Core.Util;
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

    public override void OnSpawn(GameObject spawnpoint)
    {
        base.OnSpawn(spawnpoint);

        _flower = NPCFlower.Nearest(transform.position);
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
        base.Kill();

        _harvestCooldownTimer.Stop();
        GameManager.Instance.BeetleKilled();
    }

    public static ProxPool<NPCBeetle> Pool = new ProxPool<NPCBeetle>();

    public static NPCBeetle Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist);
    }
}
