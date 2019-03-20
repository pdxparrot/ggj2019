using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Home
{
// TODO: move behavior code here
    public sealed class HiveBehavior : ActorBehavior2D
    {
        public HiveBehaviorData HiveBehaviorData => (HiveBehaviorData)BehaviorData;

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is HiveBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
