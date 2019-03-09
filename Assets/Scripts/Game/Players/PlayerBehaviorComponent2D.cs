using pdxpartyparrot.Game.Actors.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerBehavior2D))]
    public abstract class PlayerBehaviorComponent2D : CharacterBehaviorComponent2D
    {
        protected PlayerBehavior2D PlayerBehavior => (PlayerBehavior2D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerBehavior2D);
        }
#endregion
    }
}
