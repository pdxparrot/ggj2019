using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class NPCFlower : NPCBase
    {
        // TODO: pollen => health (unless spawning pollen costs pollen)
        [SerializeField]
        private int _initialPollen = 5;

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

        private TrackEntry _deathTrackEntry;

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

            _pollen = _initialPollen;

            // acquire our spawnpoints during the grow animation
            _beetleSpawn.Acquire(this, null);
            _pollenSpawn.Acquire(this, null);

            TrackEntry track = _spineAnimationHelper.SetAnimation(0, "flower_grow", false);
            track.Complete += x => {
                // now free to spawn stuff
                _beetleSpawn.Release();
                _pollenSpawn.Release();

                _spineAnimationHelper.SetAnimation(0, "flower_idle", true);
            };

            return true;
        }

        public override void OnDeSpawn()
        {
            if(null != _deathTrackEntry) {
                _deathTrackEntry.Complete -= OnDeathComplete;
                _deathTrackEntry = null;
            }

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
            if(null != _deathTrackEntry) {
                _deathTrackEntry.Complete -= OnDeathComplete;
                _deathTrackEntry = null;
            }

            IsDead = true;

            // forcefully acquire our spawnpoints (they're going away)
            _beetleSpawn.Acquire(this, null, true);
            _pollenSpawn.Acquire(this, null, true);

            _deathTrackEntry = _spineAnimationHelper.SetAnimation(0, "flower_death", false);
            _deathTrackEntry.Complete += OnDeathComplete;
        }

        private void OnDeathComplete(TrackEntry trackEntry)
        {
            base.Kill(false);
        }

        public int BeetleHarvest(int amount)
        {
            int result =  Mathf.Min(_pollen, amount);
            _pollen -= result;

            if(_pollen <= 0) {
                Kill(false);
            }

            return result;
        }
    }
}
