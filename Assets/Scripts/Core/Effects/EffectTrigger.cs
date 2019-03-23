using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects.EffectTriggerComponents;

using UnityEngine;

namespace pdxpartyparrot.Core.Effects
{
    public class EffectTrigger : MonoBehaviour
    {
// TODO: reorderable list
        [SerializeField]
        private EffectTrigger[] _triggerOnComplete;

        private Coroutine _effectWaiter;

// TODO: reorderable list (don't use GetComponents())
        private EffectTriggerComponent[] _components;

#region Unity Lifecycle
        private void Awake()
        {
            _components = GetComponents<EffectTriggerComponent>();
            //Debug.Log($"Found {_components.Length} EffectTriggerComponents");

            RunOnComponents(c => c.Initialize());
        }

        private void Update()
        {
            float dt = UnityEngine.Time.deltaTime;

            RunOnComponents(c => {
                if(!c.IsDone) {
                    c.OnUpdate(dt);
                }
            });
        }
#endregion

#region Components
        [CanBeNull]
        public T GetEffectTriggerComponent<T>() where T: EffectTriggerComponent
        {
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    return tc;
                }
            }
            return null;
        }

        public void GetEffectTriggerComponents<T>(ICollection<T> components) where T: EffectTriggerComponent
        {
            components.Clear();
            foreach(var component in _components) {
                T tc = component as T;
                if(tc != null) {
                    components.Add(tc);
                }
            }
        }

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
                    if(component.WaitForComplete && !component.IsDone) {
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

            //Debug.Log($"Trigger {_triggerOnComplete.Length} more effects from {name}");
            foreach(EffectTrigger onCompleteEffect in _triggerOnComplete) {
                onCompleteEffect.Trigger();
            }
        }
    }
}
