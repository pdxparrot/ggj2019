using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCBeetle : NPCEnemy
    {
        // TODO: NPCManager.Beetles
        private static readonly List<NPCBeetle> _beetles = new List<NPCBeetle>();

        public static IReadOnlyCollection<NPCBeetle> Beetles => _beetles;

        [SerializeField]
        private float _harvestCooldown = 1.0f;

        [SerializeField]
        [ReadOnly]
        private NPCFlower _flower;

        public int Pollen { get; private set; }

        private readonly Timer _harvestCooldownTimer = new Timer();

        private SpawnPoint _spawnpoint;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _beetles.Add(this);
        }

        protected override void OnDestroy()
        {
            _beetles.Remove(this);

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

            _flower = NPCFlower.Flowers.Nearest(transform.position);
            if(!_flower.Collides(this) || _flower.IsDead) {
                Debug.LogWarning($"Spawned on a dead / missing flower: {_flower.IsDead}");
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
    }
}
