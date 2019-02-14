using System;
using System.Collections;

using pdxpartyparrot.Core.Effects.EffectTriggerComponents;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects
{
    public class EffectTrigger : MonoBehaviour
    {
        private Coroutine _effectWaiter;

        private EffectTriggerComponent[] _components;

#region Unity Lifecycle
        private void Awake()
        {
            _components = GetComponents<EffectTriggerComponent>();
            //Debug.Log($"Found {_components.Length} EffectTriggerComponents");

            RunOnComponents(c => c.Initialize());
        }
#endregion

#region Components
        private void RunOnComponents(Action<EffectTriggerComponent> f)
        {
            foreach(var component in _components) {
                f(component);
            }
        }
#endregion

        public void Trigger(Action callback=null)
        {
            RunOnComponents(c => c.OnStart());

            _effectWaiter = StartCoroutine(EffectWaiter(callback));
        }

        // forcefully stops the trigger early
        public void StopTrigger()
        {
            if(null != _effectWaiter) {
                StopCoroutine(_effectWaiter);
                _effectWaiter = null;
            }

            RunOnComponents(c => c.OnStop());
        }

        public void ResetTrigger()
        {
            RunOnComponents(c => c.OnReset());
        }

        private IEnumerator EffectWaiter(Action callback)
        {
            WaitForSeconds wait = new WaitForSeconds(0.05f);
            while(true) {
                yield return wait;

                bool done = true;
                foreach(var component in _components) {
                    if(!component.IsDone) {
                        done = false;
                        break;
                    }
                }

                if(done) {
                    break;
                }
            }

            _effectWaiter = null;
            callback?.Invoke();
        }
    }
}
