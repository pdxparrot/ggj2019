using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    [RequireComponent(typeof(CharacterBehavior2D))]
    public abstract class CharacterActorControllerComponent2D : CharacterActorControllerComponent
    {
        protected CharacterBehavior2D Behavior { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Behavior = GetComponent<CharacterBehavior2D>();
        }
#endregion
    }
}
