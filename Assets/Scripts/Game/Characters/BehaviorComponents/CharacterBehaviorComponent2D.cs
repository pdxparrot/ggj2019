using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    [RequireComponent(typeof(CharacterBehavior2D))]
    public abstract class CharacterBehaviorComponent2D : CharacterBehaviorComponent
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
