using JetBrains.Annotations;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Game.Characters.NPCs;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.Game.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public abstract class NPC : NPC2D
    {
        public NPCBehavior GameNPCBehavior => (NPCBehavior)NPCBehavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is NPCBehavior);
        }
#endregion

        public void Kill([CanBeNull] IPlayer player)
        {
            GameNPCBehavior.OnKill(player);
        }
    }
}
