using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using Spine;
using Spine.Unity;
using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCFlower : NPCBase
    {
        // TODO: NPCManager.Flowers
        private static readonly List<NPCFlower> _flowers = new List<NPCFlower>();

        public static IReadOnlyCollection<NPCFlower> Flowers => _flowers;

        [SerializeField]
        private int _initialPollen = 5;

        [SerializeField]
        private int _pollenRate = 1;

        [SerializeField]
        [ReadOnly]
        private int _pollen;

        public int Pollen => _pollen;

        public bool HasPollen => Pollen > 0;

        public bool IsReady { get; private set; }

        [SerializeField]
        [ReadOnly]
        private bool _canSpawnPollen = true;

        public bool CanSpawnPollen
        {
            get => _canSpawnPollen;
            set => _canSpawnPollen = value;
        }

        private SpawnPoint _spawnpoint;

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            Model = Instantiate(GameManager.Instance.GameGameData.FlowerPrefabs.GetRandomEntry(), transform);
            _animation = Model.GetComponent<SkeletonAnimation>();

            if(!spawnpoint.Acquire(this)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                _pooledObject.Recycle();
                return;
            }
            _spawnpoint = spawnpoint;

            _flowers.Add(this);

            _pollen = _initialPollen;

            IsReady = false;

            TrackEntry track = SetAnimation(0, "flower_grow", false);
            track.Complete += x => {
                IsReady = true;
                SetAnimation(0, "flower_idle", true);
            };
        }

        protected override void OnDeSpawn()
        {
            _flowers.Remove(this);

            _animation = null;
            if(null != Model) {
                Destroy(Model);
                Model = null;
            }

            base.OnDeSpawn();
        }

        public void SpawnPollen()
        {
// TODO: pool
            Instantiate(GameManager.Instance.GameGameData.PollenPrefab, transform.position, Quaternion.identity);
        }

        public override void Kill()
        {
            IsDead = true;

            TrackEntry track = SetAnimation(0, "flower_death", false);
            track.Complete += x => {
                _spawnpoint.Release();
                _spawnpoint = null;

                base.Kill();
            };
        }

        public int BeetleHarvest()
        {
            int result =  Mathf.Min(_pollen, _pollenRate);
            _pollen -= result;

            if(_pollen <= 0) {
                Kill();
            }

            return result;
        }
    }
}
