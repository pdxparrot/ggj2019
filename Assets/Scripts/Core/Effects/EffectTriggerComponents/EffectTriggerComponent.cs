using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    [RequireComponent(typeof(EffectTrigger))]
    public abstract class EffectTriggerComponent : MonoBehaviour
    {
        [SerializeField]
        private bool _waitForComplete = true;

        public bool WaitForComplete => _waitForComplete;

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
    }
}
