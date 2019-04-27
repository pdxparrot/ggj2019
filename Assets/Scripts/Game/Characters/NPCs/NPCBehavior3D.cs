using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.NPCs
{
    public abstract class NPCBehavior3D : CharacterBehavior3D, INPCBehavior
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
            Movement3D.AngularVelocity = Vector3.zero;
        }
#endregion

#region Events
        public virtual void OnRecycle()
        {
        }
#endregion
    }
}
