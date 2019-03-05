using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    [RequireComponent(typeof(CharacterActorController3D))]
    public abstract class CharacterActorControllerComponent3D : CharacterActorControllerComponent
    {
        protected CharacterActorController3D Controller { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Controller = GetComponent<CharacterActorController3D>();
        }
#endregion
    }
}
