﻿using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019;
using UnityEngine;

public class NPCBeetle : NPCEnemy
{
    [SerializeField]
    private float _harvestCooldown = 1.0f;

    public NPCFlower Flower { get; private set; }

    public int Pollen { get; private set; }

    private readonly Timer _harvestCooldownTimer = new Timer();

    private void Start() {
        Pool.Add(this);
        Flower = NPCFlower.Nearest(transform.position); // TODO: we really should only pick ready flowers, oh well
        Pollen = 0;

        _harvestCooldownTimer.Start(_harvestCooldown);

        Flower.CanSpawnPollen = false;
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        Flower.CanSpawnPollen = true;

        base.OnDestroy();
    }

    void Update() {
        if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
            return;
        }

        if(null == Flower) {
            Kill();
            return;
        }

        float dt = Time.deltaTime;

        _harvestCooldownTimer.Update(dt);

        if(_harvestCooldownTimer.IsRunning) {
            return;
        }

        Pollen += Flower.Harvest(true);
        if(Flower.IsDead) {
            Kill();
            return;
        }

        _harvestCooldownTimer.Start(_harvestCooldown);
    }

    public static ProxPool<NPCBeetle> Pool = new ProxPool<NPCBeetle>();

    public static NPCBeetle Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCBeetle;
    }
}
