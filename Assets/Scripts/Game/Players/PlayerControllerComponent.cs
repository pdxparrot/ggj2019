using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerController))]
    public abstract class PlayerControllerComponent : CharacterActorControllerComponent
    {
        protected PlayerController PlayerController => (PlayerController)Controller;
    }
}
