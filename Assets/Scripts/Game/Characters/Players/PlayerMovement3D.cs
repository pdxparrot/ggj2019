using pdxpartyparrot.Core.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters
{
    public class PlayerMovement3D : CharacterMovement3D
    {
        protected override void InitRigidbody(Rigidbody rb, ActorBehaviorData behaviorData)
        {
            base.InitRigidbody(rb, behaviorData);

            // we run the follow cam in FixedUpdate() and interpolation interferes with that
            rb.interpolation = RigidbodyInterpolation.None;
        }
    }
}
