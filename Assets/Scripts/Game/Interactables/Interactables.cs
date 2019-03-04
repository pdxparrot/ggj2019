using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Collections;

using UnityEngine;

namespace pdxpartyparrot.Game.Interactables
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactables : MonoBehaviour
    {
#region Events
        public event EventHandler<InteractableEventArgs> InteractableAddedEvent;
        public event EventHandler<InteractableEventArgs> InteractableRemovedEvent;
#endregion

        private Collider2D _trigger;

        private readonly Dictionary<Type, HashSet<IInteractable>> _interactables = new Dictionary<Type, HashSet<IInteractable>>();

#region Unity Life Cycle
        private void Awake()
        {
            _trigger = GetComponent<Collider2D>();
            _trigger.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if(null == interactable || !interactable.CanInteract) {
                return;
            }

            var interactables = _interactables.GetOrAdd(interactable.GetType());
            if(interactables.Add(interactable)) {
                InteractableAddedEvent?.Invoke(this, new InteractableEventArgs{
                    Interactable = interactable
                });
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if(null == interactable) {
                return;
            }

            if(RemoveInteractable(interactable)) {
                InteractableRemovedEvent?.Invoke(this, new InteractableEventArgs{
                    Interactable = interactable
                });
            }
        }
#endregion

        public bool RemoveInteractable(IInteractable interactable)
        {
            var interactables = _interactables.GetOrAdd(interactable.GetType());
            return interactables.Remove(interactable);
        }

        public IReadOnlyCollection<IInteractable> GetInteractables<T>() where T: IInteractable
        {
            return _interactables.GetOrAdd(typeof(T));
        }
    }
}
