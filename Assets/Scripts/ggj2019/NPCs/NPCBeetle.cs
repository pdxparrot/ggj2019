﻿using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCBeetle : NPCEnemy
    {
        [SerializeField]
        private float _harvestCooldown = 1.0f;

        [SerializeField]
        [ReadOnly]
        private NPCFlower _flower;

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnpoint;

        private readonly Timer _harvestCooldownTimer = new Timer();

#region Unity Lifecycle
        private void Update()
        {
            if(IsDead) {
                return;
            }

            float dt = Time.deltaTime;

            _harvestCooldownTimer.Update(dt);
        }
#endregion

        public override void Initialize(NPCData data)
        {
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                PooledObject.Recycle();
                return;
            }
            _spawnpoint = spawnpoint;

            _flower = _spawnpoint.GetComponentInParent<NPCFlower>();
            Assert.IsFalse(_flower.IsDead);
            _flower.AcquirePollenSpawnpoint(this);

            _harvestCooldownTimer.Start(_harvestCooldown, HarvestFlower);

            SetIdleAnimation();
        }

        public override void OnDeSpawn()
        {
            _harvestCooldownTimer.Stop();

            if(_flower != null) {
                _flower.ReleasePollenSpawnpoint();
                _flower = null;
            }

            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            base.OnDeSpawn();
        }

        private void SetIdleAnimation()
        {
            SetAnimation("beetle_idle", true);
        }

        private void HarvestFlower()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            _flower.BeetleHarvest(Damage);
            if(_flower.IsDead) {
                _flower = null;

                Kill(false);
                return;
            }

            _harvestCooldownTimer.Start(_harvestCooldown, HarvestFlower);
        }

        public override void Kill(bool playerKill)
        {
            if(playerKill) {
                GameManager.Instance.BeetleKilled();
            }

            base.Kill(playerKill);
        }
    }
}
