using pdxpartyparrot.Core.Actors;

using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Actors
{
    public abstract class CharacterDriver : ActorDriver
    {
        protected ICharacterBehavior CharacterBehavior => (ICharacterBehavior)Behavior;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsTrue(Behavior is ICharacterBehavior);
        }
#endregion
    }
}
