using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    // TODO: make core-ish
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public abstract class NPCBase : Actor2D, INPC
    {
        public override bool IsLocalActor => false;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        [SerializeField]
        private EffectTrigger _spawnEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        [SerializeField]
        [ReadOnly]
        private NPCData _npcData;

        protected NPCData NPCData => _npcData;

        private PooledObject _pooledObject;

        protected SpineAnimationHelper _spineAnimationHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;

            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            IsDead = false;

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

        public virtual void Initialize(NPCData data)
        {
            _npcData = data;
        }

        // TODO: name this something else so it's less-death specific
        // like maybe just "Despawn()" or something?
        public virtual void Kill(bool playerKill)
        {
            if(IsDead) {
                return;
            }

            IsDead = true;

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
            _pooledObject.Recycle();
        }
#endregion
    }
}
