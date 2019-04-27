using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public abstract class NPCBehavior : NPCBehavior2D
    {
        [Space(10)]

        [Header("NPC")]

#region Effects
        [SerializeField]
        private EffectTrigger _deathEffect;
#endregion

        public virtual bool IsDead { get; }

#region Events
        public virtual void OnKill([CanBeNull] Player player)
        {
            if(IsDead) {
                return;
            }

            _deathEffect.Trigger(OnDeathComplete);
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            // reset this before calling OnSpawn so that we don't interfere
            // with spawn effect animations (they share the same helper)
            _deathEffect.ResetTrigger();

            base.OnSpawn(spawnpoint);

            Movement2D.Velocity = Vector3.zero;
        }

        public override void OnDeSpawn()
        {
            Movement2D.Velocity = Vector3.zero;

            _deathEffect.StopTrigger();

            base.OnDeSpawn();
        }
#endregion

#region Event Handlers
        protected virtual void OnDeathComplete()
        {
            NPC.Recycle();
        }
#endregion
    }
}
