using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerController3D))]
    public abstract class PlayerControllerComponent3D : CharacterActorControllerComponent3D
    {
        protected PlayerController3D PlayerController => (PlayerController3D)Controller;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Controller is PlayerController3D);
        }
#endregion
    }
}
