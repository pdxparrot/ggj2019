using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Game.NPCs;

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
    }
}
