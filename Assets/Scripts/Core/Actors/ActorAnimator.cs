using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorAnimator : MonoBehaviour
    {
        public abstract bool IsAnimating { get; }

        [SerializeField]
        private ActorBehavior _behavior;

        protected ActorBehavior Behavior => _behavior;

#region Unity Lifecycle
        private void Update()
        {
            float dt = UnityEngine.Time.deltaTime;

            UpdateAnimation(dt);
        }
#endregion

        protected abstract void UpdateAnimation(float dt);
    }
}
