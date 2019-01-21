using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class MainMenu : MenuPanel
    {
        [SerializeField]
        private Button _multiplayerButton;

        [SerializeField]
        private MultiplayerMenu _multiplayerPanel;

        [SerializeField]
        private CreditsMenu _creditsPanel;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            if(null!= _multiplayerButton && !Application.isEditor) {
                _multiplayerButton.gameObject.SetActive(false);
            }

            if(null != _multiplayerPanel) {
                _multiplayerPanel.gameObject.SetActive(false);
            }

            InitDebugMenu();
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();
        }
#endregion

#region Event Handlers
        public void OnPlay()
        {
            GameStateManager.Instance.StartSinglePlayer();
        }

        public void OnMultiplayer()
        {
            Owner.PushPanel(_multiplayerPanel);
        }

        public void OnCredits()
        {
            Owner.PushPanel(_creditsPanel);
        }

        public void OnQuitGame()
        {
            UnityUtil.Quit();
        }
#endregion

        private void InitDebugMenu()
        {
// TODO: this should change depending on if we're hosting/joining or whatever
// so that we don't get into a fucked up state
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Multiplayer Menu");
            _debugMenuNode.RenderContentsAction = () => {
                if(GUIUtils.LayoutButton("Host")) {
                    GameStateManager.Instance.StartHost();
                    return;
                }

                if(GUIUtils.LayoutButton("Join")) {
                    GameStateManager.Instance.StartJoin();
                    return;
                }
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
