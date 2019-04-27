using pdxpartyparrot.Core.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters
{
    public class PlayerMovement2D : CharacterMovement2D
    {
        protected override void InitRigidbody(Rigidbody2D rb, ActorBehaviorData behaviorData)
        {
            base.InitRigidbody(rb, behaviorData);

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation2D.None;
        }
    }
}
