using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerController3D))]
    public abstract class PlayerControllerComponent3D : CharacterActorControllerComponent3D
    {
        protected PlayerController3D PlayerBehavior => (PlayerController3D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerController3D);
        }
#endregion
    }
}
