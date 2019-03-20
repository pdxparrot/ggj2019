using System;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    public sealed class Wasp : Enemy
    {
        private WaspBehavior WaspBehavior => (WaspBehavior)GameNPCBehavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is WaspBehavior);
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is WaspData);

            base.Initialize(id, data);
        }
    }
}
