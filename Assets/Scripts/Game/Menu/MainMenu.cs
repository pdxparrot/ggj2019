using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    public abstract class MainMenu : MenuPanel
    {
        [SerializeField]
        private Button _multiplayerButton;

        [SerializeField]
        private MultiplayerMenu _multiplayerPanel;

        [SerializeField]
        private HighScoresMenu _highScoresPanel;

        [SerializeField]
        private CreditsMenu _creditsPanel;

        protected abstract bool UseMultiplayer { get; }

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            if(UseMultiplayer && null != _multiplayerButton && Application.isEditor) {
                _multiplayerButton.gameObject.SetActive(true);
            }

            if(null != _multiplayerPanel) {
                _multiplayerPanel.gameObject.SetActive(false);
            }

            if(null != _highScoresPanel) {
                _highScoresPanel.gameObject.SetActive(false);
            }

            if(null != _creditsPanel) {
                _creditsPanel.gameObject.SetActive(false);
            }

            InitDebugMenu();
        }

        protected virtual void OnDestroy()
        {
            DestroyDebugMenu();
        }
#endregion

#region Event Handlers
        // these are all just demo methods
        public void OnPlay()
        {
            // TODO: this takes in the main game state now
            //GameStateManager.Instance.StartLocal();
        }

        public void OnMultiplayer()
        {
            Owner.PushPanel(_multiplayerPanel);
        }

        public void OnHighScores()
        {
            Owner.PushPanel(_highScoresPanel);
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
            /*if(UseMultiplayer) {
// TODO: this should change depending on if we're hosting/joining or whatever
// so that we don't get into a fucked up state
                _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Multiplayer Menu");
                _debugMenuNode.RenderContentsAction = () => {
                    // TODO: these take in the main game state now
                    if(GUIUtils.LayoutButton("Host")) {
                        GameStateManager.Instance.StartHost();
                        return;
                    }

                    if(GUIUtils.LayoutButton("Join")) {
                        GameStateManager.Instance.StartJoin();
                        return;
                    }
                };
            }*/
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
