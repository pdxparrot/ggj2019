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

        // NOTE: axes are (x, y, 0)
        public virtual bool OnAnimationMove(Vector3 axes, float dt)
        {
            return false;
        }

        // NOTE: axes are (x, y, 0)
        public virtual bool OnPhysicsMove(Vector3 axes, float speed, float dt)
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
