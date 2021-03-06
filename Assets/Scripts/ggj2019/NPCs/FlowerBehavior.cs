﻿using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class FlowerBehavior : NPCBehavior
    {
        [Space(10)]

        [Header("Flower")]

#region State
// TODO: these could merge into a state enum probably
        [SerializeField]
        [ReadOnly]
        private bool _canSpawnPollen;

        [SerializeField]
        [ReadOnly]
        private bool _isDead = true;

        public override bool IsDead => _isDead;
#endregion

        [SerializeField]
        [ReadOnly]
        private int _pollen;

        private ITimer _pollenSpawnTimer;

        private Flower FlowerNPC => (Flower)NPC;

        public FlowerBehaviorData FlowerBehaviorData => (FlowerBehaviorData)NPCBehaviorData;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pollenSpawnTimer = TimeManager.Instance.AddTimer();
            _pollenSpawnTimer.TimesUpEvent += PollSpawnTimerTimesUpEventHandler;
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_pollenSpawnTimer);
            }
            _pollenSpawnTimer = null;

            base.OnDestroy();
        }
#endregion

#region Actions
        private void SpawnPollen()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            if(DoSpawnPollen()) {
                _pollen--;
                if(_pollen <= 0) {
                    FlowerNPC.Kill(null);
                    return;
                }
            }

            _pollenSpawnTimer.Start(FlowerBehaviorData.PollenSpawnCooldown.GetRandomValue());
        }

        private bool DoSpawnPollen()
        {
            if(!_canSpawnPollen || ActorManager.Instance.ActorCount<Pollen>() >= FlowerBehaviorData.MaxPollen) {
                return false;
            }

            Pollen pollen = ObjectPoolManager.Instance.GetPooledObject<Pollen>("pollen");
            FlowerNPC.SpawnPollen(pollen, FlowerBehaviorData.PollenBehaviorData);

            return true;
        }
#endregion

#region Events
        public override void OnKill(Player player)
        {
            // forcefully acquire our spawnpoints while we die
            FlowerNPC.AcquireBeetleSpawnpoint(true);
            _canSpawnPollen = false;

            base.OnKill(player);

            _isDead = true;
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            _isDead = false;

             FlowerNPC.SetRandomSkin();

            // acquire our spawnpoints while we spawn
            FlowerNPC.AcquireBeetleSpawnpoint(true);
            _canSpawnPollen = false;

            _pollen = FlowerBehaviorData.Pollen;
        }

        protected override void OnSpawnComplete()
        {
            base.OnSpawnComplete();

            // now free to spawn stuff
            FlowerNPC.ReleaseBeetleSpawnpoint();
            _canSpawnPollen = true;

            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(FlowerBehaviorData.IdleAnimationName, true);
            }

            _pollenSpawnTimer.Start(FlowerBehaviorData.PollenSpawnCooldown.GetRandomValue());
        }

        public override void OnDeSpawn()
        {
            _pollenSpawnTimer.Stop();

            // need to release these before the spawnpoints disable
            FlowerNPC.ReleaseBeetleSpawnpoint();
            _canSpawnPollen = false;

            base.OnDeSpawn();
        }

        public void OnAcquirePollenSpawnpoint()
        {
            _canSpawnPollen = false;
        }

        public void OnReleasePollenSpawnpoint()
        {
            _canSpawnPollen = true;
        }

        public void OnBeetleHarvest(Beetle beetle)
        {
            _pollen--;
            if(_pollen <= 0) {
                GameManager.Instance.FlowerDestroyed(FlowerNPC);

                FlowerNPC.Kill(null);
                return;
            }

            GameManager.Instance.BeetleHarvest(beetle, 1);
        }
#endregion

#region Event Handlers
        private void PollSpawnTimerTimesUpEventHandler(object sender, EventArgs args)
        {
            SpawnPollen();
        }
#endregion
    }
}
