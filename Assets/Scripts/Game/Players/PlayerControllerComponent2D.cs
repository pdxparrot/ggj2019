using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerController2D))]
    public abstract class PlayerControllerComponent2D : CharacterActorControllerComponent2D
    {
        protected PlayerController2D PlayerController => (PlayerController2D)Controller;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Controller is PlayerController2D);
        }
#endregion
    }
}
