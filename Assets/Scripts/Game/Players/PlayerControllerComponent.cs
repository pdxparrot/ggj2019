using pdxpartyparrot.Game.Actors.ControllerComponents;

using UnityEngine;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(IPlayerController))]
    public abstract class PlayerControllerComponent : CharacterActorControllerComponent
    {
        protected IPlayerController PlayerController => (IPlayerController)Controller;
    }
}
