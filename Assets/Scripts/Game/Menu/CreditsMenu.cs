using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Menu
{
// TODO: this should populate some sort of UI from the credits data

    public sealed class CreditsMenu : MenuPanel, ICancelActionHandler
    {
        [SerializeField]
        private CreditsData _creditsData;

#region Unity Lifecycle
        private void OnEnable()
        {
            InputManager.Instance.EventSystem.UIModule.cancel.action.performed += OnCancel;
        }

        private void OnDisable()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.EventSystem.UIModule.cancel.action.performed -= OnCancel;
            }
        }
#endregion

#region Event Handlers
        public void OnBack()
        {
            Owner.PopPanel();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            OnBack();
        }
#endregion
    }
}
