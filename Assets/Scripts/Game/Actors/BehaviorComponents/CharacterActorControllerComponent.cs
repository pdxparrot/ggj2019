using UnityEngine;

namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    public abstract class CharacterActorControllerComponent : MonoBehaviour
    {
// TODO: if subclasses could register for specific action types (and we keep a dictionary ActionType => Listener)
// then that would work out a lot faster and cleaner than how this is currently done

#region Actions
        public abstract class CharacterActorControllerAction
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

        public virtual bool OnStarted(CharacterActorControllerAction action)
        {
            return false;
        }

        public virtual bool OnPerformed(CharacterActorControllerAction action)
        {
            return false;
        }

        public virtual bool OnCancelled(CharacterActorControllerAction action)
        {
            return false;
        }
    }
}
