using pdxpartyparrot.Game.Actors.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.NPCs.BehaviorComponents
{
    [RequireComponent(typeof(NPCBehavior2D))]
    public abstract class NPCBehaviorComponent2D : CharacterBehaviorComponent2D
    {
        protected NPCBehavior2D NPCBehavior => (NPCBehavior2D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is NPCBehavior2D);
        }
#endregion
    }
}
