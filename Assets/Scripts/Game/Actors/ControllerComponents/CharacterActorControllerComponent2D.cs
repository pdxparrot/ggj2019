using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    [RequireComponent(typeof(CharacterActorController2D))]
    public abstract class CharacterActorControllerComponent2D : CharacterActorControllerComponent
    {
        protected CharacterActorController2D Controller { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Controller = GetComponent<CharacterActorController2D>();
        }
#endregion
    }
}
