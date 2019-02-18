using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class NPCFlower : NPCBase
    {
        [SerializeField]
        private SpawnPoint _beetleSpawn;

        [SerializeField]
        private SpawnPoint _pollenSpawn;

        [SerializeField]
        [ReadOnly]
        private int _pollen;

        public int Pollen => _pollen;

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnpoint;

        public bool CanSpawnPollen => !IsDead && Pollen > 0;

        private SpineSkinSwapper _skinSwapper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _skinSwapper = GetComponent<SpineSkinSwapper>();
        }
#endregion

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                return false;
            }
            _spawnpoint = spawnpoint;

             _skinSwapper.SetRandomSkin();

            // acquire our spawnpoints while we spawn
            _beetleSpawn.Acquire(this, null);
            _pollenSpawn.Acquire(this, null);

            _pollen = GameManager.Instance.GameGameData.FlowerPollen;

            return true;
        }

        public override void OnDeSpawn()
        {
            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            // need to release these before the spawnpoints disable
            _beetleSpawn.Release();
            _pollenSpawn.Release();

            base.OnDeSpawn();
        }

        public override void Initialize(NPCData data)
        {
            base.Initialize(data);
        }

        public void SpawnPollen()
        {
            _pollen--;
            if(_pollen <= 0) {
                Kill(false);
            }
        }

        public void AcquirePollenSpawnpoint(NPCBase owner)
        {
            _pollenSpawn.Acquire(owner, null);
        }

        public void ReleasePollenSpawnpoint()
        {
            _pollenSpawn.Release();
        }

        public override void Kill(bool playerKill)
        {
            base.Kill(playerKill);

            // forcefully acquire our spawnpoints while we die
            _beetleSpawn.Acquire(this, null, true);
            _pollenSpawn.Acquire(this, null, true);
        }

        public bool BeetleHarvest()
        {
            _pollen--;

            if(_pollen <= 0) {
                Kill(false);
                return true;
            }

            return false;
        }

        protected override void OnSpawnComplete()
        {
            base.OnSpawnComplete();

            // now free to spawn stuff
            _beetleSpawn.Release();
            _pollenSpawn.Release();

            _spineAnimationHelper.SetAnimation(0, "flower_idle", true);
        }
    }
}
