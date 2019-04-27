using pdxpartyparrot.Game.Characters.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.NPCs.BehaviorComponents
{
    [RequireComponent(typeof(NPCBehavior2D))]
    public abstract class NPCBehaviorComponent2D : CharacterBehaviorComponent2D
    {
        protected NPCBehavior2D NPCBehavior => (NPCBehavior2D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Behavior is NPCBehavior2D);

            base.Awake();
        }
#endregion
    }
}
