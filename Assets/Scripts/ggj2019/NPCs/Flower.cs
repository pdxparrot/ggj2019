﻿using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class Flower : NPC2D
    {
#region Spawn Points
        [SerializeField]
        private SpawnPoint _beetleSpawn;

        [SerializeField]
        private SpawnPoint _pollenSpawn;
#endregion

        [SerializeField]
        [ReadOnly]
        private int _pollen;

        [SerializeField]
        [ReadOnly]
        private bool _canSpawnPollen;

        [SerializeField]
        [ReadOnly]
        private bool _isDead = true;

        public bool IsDead => _isDead;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _pollenSpawnTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnpoint;

        private FlowerData FlowerData => (FlowerData)NPCData;

        private SpineAnimationHelper _spineAnimationHelper;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is FlowerBehavior);

            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();
            _skinHelper = GetComponent<SpineSkinHelper>();
        }

        protected override void Update()
        {
            float dt = Time.deltaTime;

            _pollenSpawnTimer.Update(dt);

            base.Update();
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                return false;
            }
            _spawnpoint = spawnpoint;

             _skinHelper.SetRandomSkin();

            // acquire our spawnpoints while we spawn
            _beetleSpawn.Acquire(this, null);
            _canSpawnPollen = false;

            return true;
        }

        public override void OnDeSpawn()
        {
            _pollenSpawnTimer.Stop();

            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            // need to release these before the spawnpoints disable
            _beetleSpawn.Release();
            _canSpawnPollen = false;

            base.OnDeSpawn();
        }

        protected override void OnSpawnComplete()
        {
            base.OnSpawnComplete();

            // now free to spawn stuff
            _beetleSpawn.Release();
            _canSpawnPollen = true;

            _spineAnimationHelper.SetAnimation(FlowerData.IdleAnimationName, true);

            _pollenSpawnTimer.Start(FlowerData.PollenSpawnCooldown.GetRandomValue(), SpawnPollen);
        }
#endregion

        public override void Initialize(Guid id, NPCData data)
        {
            Assert.IsTrue(data is FlowerData);

            base.Initialize(id, data);

            _pollen = FlowerData.Pollen;

            _isDead = false;
        }

        public override void Kill(IPlayer player)
        {
            // forcefully acquire our spawnpoints while we die
            _beetleSpawn.Acquire(this, null, true);
            _canSpawnPollen = false;

            base.Kill(player);

            _isDead = true;
        }

        public void AcquirePollenSpawnpoint(Actor owner)
        {
            _canSpawnPollen = false;
        }

        public void ReleasePollenSpawnpoint()
        {
            _canSpawnPollen = true;
        }

        public bool BeetleHarvest(Beetle beetle)
        {
            _pollen--;
            if(_pollen <= 0) {
                GameManager.Instance.FlowerDestroyed(this);

                Kill(null);
                return true;
            }

            GameManager.Instance.BeetleHarvest(beetle, 1);

            return false;
        }

#region Actions
        private void SpawnPollen()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            if(DoSpawnPollen()) {
                _pollen--;
                if(_pollen <= 0) {
                    Kill(null);
                    return;
                }
            }

            _pollenSpawnTimer.Start(FlowerData.PollenSpawnCooldown.GetRandomValue(), SpawnPollen);
        }

        private bool DoSpawnPollen()
        {
            if(!_canSpawnPollen || ActorManager.Instance.ActorCount<Pollen>() >= FlowerData.MaxPollen) {
                return false;
            }

            Pollen pollen = ObjectPoolManager.Instance.GetPooledObject<Pollen>("pollen");
            _pollenSpawn.Spawn(pollen, Guid.NewGuid());
            pollen.transform.SetParent(GameManager.Instance.PollenContainer.transform);
            pollen.Initialize(FlowerData.PollenData);

            return true;
        }
#endregion
    }
}
