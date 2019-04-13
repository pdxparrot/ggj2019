using UnityEngine;

namespace pdxpartyparrot.Game.Characters.BehaviorComponents
{
    [RequireComponent(typeof(CharacterBehavior3D))]
    public abstract class CharacterBehaviorComponent3D : CharacterBehaviorComponent
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
