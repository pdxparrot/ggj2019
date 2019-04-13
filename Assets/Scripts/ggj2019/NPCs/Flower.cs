using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class Flower : NPC
    {
#region Spawn Points
        [SerializeField]
        private SpawnPoint _beetleSpawn;

        [SerializeField]
        private SpawnPoint _pollenSpawn;
#endregion

        [SerializeField]
        [ReadOnly]
        private SpawnPoint _spawnpoint;

        private FlowerBehavior FlowerBehavior => (FlowerBehavior)GameNPCBehavior;

        public bool IsDead => FlowerBehavior.IsDead;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is FlowerBehavior);

            _skinHelper = GetComponent<SpineSkinHelper>();
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is FlowerBehaviorData);

            base.Initialize(id, data);
        }

        public void SpawnPollen(Pollen pollen, PollenBehaviorData behaviorData)
        {
            _pollenSpawn.Spawn(pollen, Guid.NewGuid(), behaviorData);
            pollen.transform.SetParent(GameManager.Instance.PollenContainer.transform);

            pollen.Initialize(GameManager.Instance.GameGameData.PollenData);
        }

        public void AcquirePollenSpawnpoint(Actor owner)
        {
            FlowerBehavior.OnAcquirePollenSpawnpoint();
        }

        public void ReleasePollenSpawnpoint()
        {
            FlowerBehavior.OnReleasePollenSpawnpoint();
        }

        public void BeetleHarvest(Beetle beetle)
        {
            FlowerBehavior.OnBeetleHarvest(beetle);
        }

        public void AcquireBeetleSpawnpoint(bool force=false)
        {
            _beetleSpawn.Acquire(this, null, force);
        }

        public void ReleaseBeetleSpawnpoint()
        {
            _beetleSpawn.Release();
        }

#region Skin
        public void SetRandomSkin()
        {
            _skinHelper.SetRandomSkin();
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire flower spawnpoint!");
                return false;
            }
            _spawnpoint = spawnpoint;

            return base.OnSpawn(spawnpoint);
        }

        public override void OnDeSpawn()
        {
            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            base.OnDeSpawn();
        }
#endregion
    }
}
