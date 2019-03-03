using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    [RequireComponent(typeof(EffectTrigger))]
    public abstract class EffectTriggerComponent : MonoBehaviour
    {
        public abstract bool WaitForComplete { get; }

        public virtual bool IsDone => true;

        public virtual void Initialize()
        {
        }

        public abstract void OnStart();

        public virtual void OnStop()
        {
        }

        public virtual void OnReset()
        {
        }

        public virtual void OnUpdate(float dt)
        {
        }
    }
}
