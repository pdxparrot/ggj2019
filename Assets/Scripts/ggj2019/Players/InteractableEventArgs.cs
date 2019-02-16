using System;

namespace pdxpartyparrot.ggj2019.Players
{
    public class InteractableEventArgs : EventArgs
    {
        public IInteractable Interactable { get; set; }
    }
}
