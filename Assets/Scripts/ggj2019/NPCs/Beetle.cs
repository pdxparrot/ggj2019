using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public sealed class Beetle : Enemy
    {
        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private SpawnPoint _spawnpoint;

        private BeetleBehavior BeetleBehavior => (BeetleBehavior)GameNPCBehavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is BeetleBehavior);
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is BeetleData);

            base.Initialize(id, data);
        }

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire beetle spawnpoint!");
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
