using pdxpartyparrot.Game.Characters.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players.BehaviorComponents
{
    [RequireComponent(typeof(PlayerBehavior3D))]
    public abstract class PlayerBehaviorComponent3D : CharacterBehaviorComponent3D
    {
        protected PlayerBehavior3D PlayerBehavior => (PlayerBehavior3D)Behavior;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerBehavior3D);
        }
#endregion
    }
}
