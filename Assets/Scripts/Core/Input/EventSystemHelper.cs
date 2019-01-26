using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input.Plugins.UI;

namespace pdxpartyparrot.Core.Input
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(UIActionInputModule))]
    public sealed class EventSystemHelper : MonoBehaviour
    {
        public EventSystem EventSystem { get; private set; }

        public UIActionInputModule UIModule { get; private set; }

#region Unity Lifecycle
        private void Awake()
        {
            EventSystem = GetComponent<EventSystem>();
            UIModule = GetComponent<UIActionInputModule>();
        }
#endregion
    }
}
