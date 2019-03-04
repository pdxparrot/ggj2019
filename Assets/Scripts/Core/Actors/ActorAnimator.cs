using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(ActorBehavior))]
    public abstract class ActorAnimator : MonoBehaviour
    {
        public abstract bool IsAnimating { get; }

        protected ActorBehavior Behavior { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Behavior = GetComponent<ActorBehavior>();
        }

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
