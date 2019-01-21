using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class PauseMenu : MenuPanel
    {
        [SerializeField]
        private SettingsMenu _settingsMenu;

#region Unity Lifecycle
        private void Awake()
        {
            _settingsMenu.gameObject.SetActive(false);
        }
#endregion

#region Event Handlers
        public void OnSettings()
        {
            Owner.PushPanel(_settingsMenu);
        }

        public void OnExitMainMenu()
        {
            GameStateManager.Instance.TransitionToInitialState();
        }

        public void OnQuitGame()
        {
            UnityUtil.Quit();
        }
#endregion
    }
}
