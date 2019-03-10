using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.NPCs
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class NPC2D : Actor2D, INPC
    {
        public GameObject GameObject => gameObject;

#region Network
        public override bool IsLocalActor => false;
#endregion

#region Behavior
        [CanBeNull]
        public INPCBehavior NPCBehavior => (NPCBehavior2D)Behavior;
#endregion

// TODO: behavior
#region Effects
        [SerializeField]
        private EffectTrigger _spawnEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private NPCData _npcData;

        public NPCData NPCData => _npcData;

        [CanBeNull]
        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is NPCBehavior2D);

            _pooledObject = GetComponent<PooledObject>();
            if(null != _pooledObject) {
                _pooledObject.RecycleEvent += RecycleEventHandler;
            }
        }

        protected virtual void Update()
        {
            float dt = Time.deltaTime;

            Think(dt);
        }
#endregion

        public virtual void Initialize(Guid id, NPCData data)
        {
            base.Initialize(id);

            _npcData = data;
            if(null != NPCBehavior) {
                NPCBehavior.Initialize();
            }
        }

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _deathEffect.ResetTrigger();
            _spawnEffect.Trigger(OnSpawnComplete);

            return true;
        }

        public override void OnDeSpawn()
        {
            _deathEffect.StopTrigger();

            base.OnDeSpawn();
        }
#endregion

        protected virtual void Think(float dt)
        {
        }

// TODO: subclass
        // TODO: name this something else so it's less-death specific
        // like maybe just "Despawn()" or something?
        public virtual void Kill([CanBeNull] IPlayer player)
        {
/*
            if(IsDead) {
                return;
            }
*/

            _deathEffect.Trigger(OnDeathComplete);
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }

        protected virtual void OnSpawnComplete()
        {
        }

        protected virtual void OnDeathComplete()
        {
            if(null != _pooledObject) {
                _pooledObject.Recycle();
            }
        }
#endregion
    }
}
