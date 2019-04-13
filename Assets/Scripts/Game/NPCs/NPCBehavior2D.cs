using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.NPCs
{
    public abstract class NPCBehavior2D : CharacterBehavior2D, INPCBehavior
    {
        public NPCBehaviorData NPCBehaviorData => (NPCBehaviorData)BehaviorData;

        public INPC NPC => (INPC)Owner;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Owner is INPC);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // fixes sketchy rigidbody angular momentum shit
            AngularVelocity2D = 0.0f;
        }
#endregion

#region Events
        public virtual void OnRecycle()
        {
        }
#endregion
    }
}
