using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    [RequireComponent(typeof(CharacterBehavior3D))]
    public abstract class CharacterActorControllerComponent3D : CharacterActorControllerComponent
    {
        protected CharacterBehavior3D Behavior { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Behavior = GetComponent<CharacterBehavior3D>();
        }
#endregion
    }
}
