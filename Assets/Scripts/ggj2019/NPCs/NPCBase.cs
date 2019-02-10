using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.NPCs;

using Spine;
using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    // TODO: make core-ish
    [RequireComponent(typeof(PooledObject))]
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

        protected EffectTrigger SpawnEffect => _spawnEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        protected EffectTrigger DeathEffect => _deathEffect;

        [SerializeField]
        private SkeletonAnimation _animation;

        protected SkeletonAnimation Animation => _animation;

        [SerializeField]
        [ReadOnly]
        private NPCData _npcData;

        protected NPCData NPCData => _npcData;

        private PooledObject _pooledObject;

        protected PooledObject PooledObject => _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

#region Spawn
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            if(null != Model) {
                Model.SetActive(true);
            }

            IsDead = false;

            if(null != _deathEffect) {
                _deathEffect.Reset();
            }

            if(null != _spawnEffect) {
                _spawnEffect.Trigger();
            }
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
            IsDead = true;

            Model.SetActive(false);
            if(null != _deathEffect) {
                _deathEffect.Trigger(() => {
                    _pooledObject.Recycle();
                });
            } else {
                _pooledObject.Recycle();
            }
        }

        // TODO: move all the animation junk into a helper behavior
#region Animation
        protected TrackEntry SetAnimation(string animationName, bool loop)
        {
            return SetAnimation(0, animationName, loop);
        }

        protected TrackEntry SetAnimation(int track, string animationName, bool loop)
        {
            return _animation.AnimationState.SetAnimation(track, animationName, loop);
        }

        protected void SetFacing(Vector3 direction)
        {
            _animation.Skeleton.ScaleX = direction.x < 0 ? -1.0f : 1.0f;
        }
#endregion

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
