using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Data;

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

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _pollenSpawnTimer = new Timer();

        private Flower FlowerNPC => (Flower)NPC;

        public FlowerData FlowerData => (FlowerData)NPCBehaviorData;

#region Unity Lifecycle
        protected override void Update()
        {
            float dt = Time.deltaTime;

            _pollenSpawnTimer.Update(dt);

            base.Update();
        }
#endregion

        public override void Kill(IPlayer player)
        {
            // forcefully acquire our spawnpoints while we die
            FlowerNPC.AcquireBeetleSpawnpoint(true);
            _canSpawnPollen = false;

            base.Kill(player);

            _isDead = true;
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
            FlowerNPC.SpawnPollen(pollen, FlowerData.PollenData);

            return true;
        }
#endregion

#region Events
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            _isDead = false;

             FlowerNPC.SetRandomSkin();

            // acquire our spawnpoints while we spawn
            FlowerNPC.AcquireBeetleSpawnpoint(true);
            _canSpawnPollen = false;

            _pollen = FlowerData.Pollen;
        }

        protected override void OnSpawnComplete()
        {
            base.OnSpawnComplete();

            // now free to spawn stuff
            FlowerNPC.ReleaseBeetleSpawnpoint();
            _canSpawnPollen = true;

            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(FlowerData.IdleAnimationName, true);
            }

            _pollenSpawnTimer.Start(FlowerData.PollenSpawnCooldown.GetRandomValue(), SpawnPollen);
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

                Kill(null);
                return;
            }

            GameManager.Instance.BeetleHarvest(beetle, 1);
        }
#endregion
    }
}
