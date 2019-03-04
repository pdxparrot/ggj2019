using System;

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
            float dt = Time.deltaTime;

            UpdateAnimation(dt);
        }
#endregion

        public abstract void StartAnimation2D(Vector3 targetPosition, float targetRotation, float timeSeconds, Action onComplete=null);

        public abstract void StartAnimation3D(Vector3 targetPosition, Quaternion targetRotation, float timeSeconds, Action onComplete=null);

        public abstract void UpdateAnimation(float dt);
    }
}
