using pdxpartyparrot.Game.Characters.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.NPCs.BehaviorComponents
{
    [RequireComponent(typeof(NPCBehavior3D))]
    public abstract class NPCBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        protected NPCBehavior3D NPCBehavior => (NPCBehavior3D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            Assert.IsTrue(Behavior is NPCBehavior3D);

            base.Awake();
        }
#endregion
    }
}
