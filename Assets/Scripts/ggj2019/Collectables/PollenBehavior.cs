using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Collectables
{
// TODO: move behavior code here
    public sealed class PollenBehavior : ActorBehavior2D
    {
        public PollenBehaviorData PollenBehaviorData => (PollenBehaviorData)BehaviorData;

        public override void Initialize(ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is PollenBehaviorData);

            base.Initialize(behaviorData);
        }
    }
}
