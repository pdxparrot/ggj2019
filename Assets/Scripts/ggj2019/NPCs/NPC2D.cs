using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.NPCs;
using pdxpartyparrot.Game.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    // TODO: make core-ish
    // TODO: some of this stuff could also go into the base player classes (like effects and state and shit)
    // TODO: also we could do some if USE_SPINE for the player as well
#if USE_SPINE
    [RequireComponent(typeof(SpineAnimationHelper))]
#endif
    public abstract class NPC2D : Actor2D, INPC
    {
        public override bool IsLocalActor => false;

        public abstract bool IsDead { get; }

#region Effects
        [SerializeField]
        private EffectTrigger _spawnEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private NPCData _npcData;

        protected NPCData NPCData => _npcData;

        [CanBeNull]
        private PooledObject _pooledObject;

#if USE_SPINE
        protected SpineAnimationHelper _spineAnimationHelper;
#endif

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pooledObject = GetComponent<PooledObject>();
            if(null != _pooledObject) {
                _pooledObject.RecycleEvent += RecycleEventHandler;
            }

#if USE_SPINE
            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();
#endif
        }

        protected virtual void Update()
        {
            float dt = Time.deltaTime;

            Think(dt);
        }
#endregion

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

        public virtual void Initialize(NPCData data)
        {
            _npcData = data;
        }

        // TODO: name this something else so it's less-death specific
        // like maybe just "Despawn()" or something?
        public virtual void Kill([CanBeNull] IPlayer player)
        {
            if(IsDead) {
                return;
            }

            _deathEffect.Trigger(OnDeathComplete);
        }

        protected virtual void Think(float dt)
        {
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
