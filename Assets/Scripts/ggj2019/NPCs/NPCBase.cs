using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Effects;

using Spine;
using Spine.Unity;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    public abstract class NPCBase : Actor2D
    {
        public override bool IsLocalActor => true;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        [SerializeField]
        protected EffectTrigger _spawnEffect;

        [SerializeField]
        protected EffectTrigger _deathEffect;

        [SerializeField]
        protected SkeletonAnimation _animation;

        protected PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

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

        public virtual void Kill()
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

        protected TrackEntry SetAnimation(string animationName, bool loop)
        {
            return SetAnimation(0, animationName, loop);
        }

        protected TrackEntry SetAnimation(int track, string animationName, bool loop)
        {
            return _animation.AnimationState.SetAnimation(track, animationName, loop);
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
