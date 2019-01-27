using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class PauseMenu : MenuPanel, ICancelActionHandler
    {
        [SerializeField]
        private SettingsMenu _settingsMenu;

#region Unity Lifecycle
        private void Awake()
        {
            _settingsMenu.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            // TODO: this only works the first time? wtf...
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
        public void OnSettings()
        {
            Owner.PushPanel(_settingsMenu);
        }

        public void OnBack()
        {
            PartyParrotManager.Instance.TogglePause();
        }

        public void OnExitMainMenu()
        {
            GameStateManager.Instance.TransitionToInitialState();
        }

        public void OnQuitGame()
        {
            UnityUtil.Quit();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            OnBack();
        }
#endregion
    }
}
