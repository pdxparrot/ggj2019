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

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _flowers.Add(this);

            _pollen = _initialPollen;
        }

        protected override void OnDestroy()
        {
            _flowers.Remove(this);

            _animation = null;
            if(null != Model) {
                Destroy(Model);
                Model = null;
            }

            base.OnDestroy();
        }
#endregion

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            Model = Instantiate(GameManager.Instance.GameGameData.FlowerPrefabs.GetRandomEntry(), transform);
            _animation = Model.GetComponent<SkeletonAnimation>();

            if(!spawnpoint.Acquire(this)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                Destroy(gameObject);
                return;
            }
            _spawnpoint = spawnpoint;

            IsReady = false;

            TrackEntry track = SetAnimation(0, "flower_grow", false);
            track.Complete += x => {
                IsReady = true;
                SetAnimation(0, "flower_idle", true);
            };
        }

        public void SpawnPollen()
        {
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
