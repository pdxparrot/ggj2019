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
    public abstract class NPCBase : PhysicsActor2D
    {
        public override float Height => Collider.bounds.size.y / 2.0f;

        public override float Radius => Collider.bounds.size.x / 2.0f;

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

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            IsDead = false;

            if(null != _spawnEffect) {
                _spawnEffect.Trigger();
            }
        }

        public override void OnReSpawn(SpawnPoint spawnpoint)
        {
            base.OnReSpawn(spawnpoint);

            IsDead = false;

            if(null != _spawnEffect) {
                _spawnEffect.Trigger();
            }
        }

        public virtual void Kill()
        {
            IsDead = true;

            Model.SetActive(false);
            if(null != _deathEffect) {
                _deathEffect.Trigger(() => {
                    Destroy(gameObject);
                });
            } else {
                Destroy(gameObject);
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
    }
}
