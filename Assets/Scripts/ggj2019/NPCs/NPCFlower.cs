using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Collectable;
using pdxpartyparrot.ggj2019.Players;

using Spine;
using Spine.Unity;

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

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnpoint;

        private SpineSkinSwapper _skinSwapper;

        private TrackEntry _deathTrackEntry;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _skinSwapper = GetComponent<SpineSkinSwapper>();
        }
#endregion

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            // TODO: _skinSwapper.SetRandomSkin();
            Model = Instantiate(GameManager.Instance.GameGameData.FlowerPrefabs.GetRandomEntry(), transform);

            // TODO: move this to awake once we know we have a model
            _animation = Model.GetComponent<SkeletonAnimation>();

            base.OnSpawn(spawnpoint);

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                _pooledObject.Recycle();
                return;
            }
            _spawnpoint = spawnpoint;

            _pollen = _initialPollen;

            // acquire our spawnpoints during the grow animation
            _beetleSpawn.Acquire(this, null);
            _pollenSpawn.Acquire(this, null);

            TrackEntry track = SetAnimation(0, "flower_grow", false);
            track.Complete += x => {
                // now free to spawn stuff
                _beetleSpawn.Release();
                _pollenSpawn.Release();

                SetAnimation(0, "flower_idle", true);
            };
        }

        protected override void OnDeSpawn()
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

            // TODO: remove this when skins are in
            _animation = null;
            if(null != Model) {
                Destroy(Model);
                Model = null;
            }
        }

        public void SpawnPollen()
        {
            _pollen--;
            if(_pollen <= 0) {
                Kill();
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

        public override void Kill()
        {
            if(null != _deathTrackEntry) {
                _deathTrackEntry.Complete -= OnDeathComplete;
                _deathTrackEntry = null;
            }

            IsDead = true;

            // forcefully acquire our spawnpoints (they're going away)
            _beetleSpawn.Acquire(this, null, true);
            _pollenSpawn.Acquire(this, null, true);

            _deathTrackEntry = SetAnimation(0, "flower_death", false);
            _deathTrackEntry.Complete += OnDeathComplete;
        }

        private void OnDeathComplete(TrackEntry trackEntry)
        {
            base.Kill();
        }

        public int BeetleHarvest(int amount)
        {
            int result =  Mathf.Min(_pollen, amount);
            _pollen -= result;

            if(_pollen <= 0) {
                Kill();
            }

            return result;
        }
    }
}
