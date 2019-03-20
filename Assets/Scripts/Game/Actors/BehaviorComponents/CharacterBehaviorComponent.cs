using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    public abstract class CharacterBehaviorComponent : MonoBehaviour
    {
// TODO: if subclasses could register for specific action types (and we keep a dictionary ActionType => Listener)
// then that would work out a lot faster and cleaner than how this is currently done

#region Actions
        public abstract class CharacterBehaviorAction
        {
        }
#endregion

#region Unity Lifecycle
        protected virtual void Awake()
        {
        }

        protected virtual void OnDestroy()
        {
        }
#endregion

        public virtual bool OnAnimationMove(Vector2 direction, float dt)
        {
            return false;
        }

        public virtual bool OnPhysicsMove(Vector2 direction, float speed, float dt)
        {
            return false;
        }

        public virtual bool OnStarted(CharacterBehaviorAction action)
        {
            return false;
        }

        public virtual bool OnPerformed(CharacterBehaviorAction action)
        {
            return false;
        }

        public virtual bool OnCancelled(CharacterBehaviorAction action)
        {
            return false;
        }
    }
}
