using pdxpartyparrot.Game.Actors.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(PlayerController2D))]
    public abstract class PlayerControllerComponent2D : CharacterActorControllerComponent2D
    {
        protected PlayerController2D PlayerBehavior => (PlayerController2D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerController2D);
        }
#endregion
    }
}
