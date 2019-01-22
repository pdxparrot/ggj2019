using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Menu;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    public sealed class UIManager : SingletonBehavior<UIManager>
    {
        [SerializeField]
        private Menu.Menu _pauseMenuPrefab;

        [CanBeNull]
        private Menu.Menu _pauseMenu;

        private GameObject _uiContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _uiContainer = new GameObject("UI");

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }

            Destroy(_uiContainer);
            _uiContainer = null;

            base.OnDestroy();
        }
#endregion

        public void Initialize()
        {
            _pauseMenu = InstantiateUIPrefab(_pauseMenuPrefab);
            if(null != _pauseMenu) {
                _pauseMenu.gameObject.SetActive(PartyParrotManager.Instance.IsPaused);
            }
        }

        public void Shutdown()
        {
            if(null != _pauseMenu) {
                Destroy(_pauseMenu.gameObject);
            }
            _pauseMenu = null;
        }

        public TV InstantiateUIPrefab<TV>(TV prefab) where TV: Component
        {
            return Instantiate(prefab, _uiContainer.transform);
        }

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(null == _pauseMenu) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused) {
                _pauseMenu.gameObject.SetActive(true);
                _pauseMenu.ResetMenu();
            } else {
                _pauseMenu.gameObject.SetActive(false);
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.UIManager");
            debugMenuNode.RenderContentsAction = () => {
            };
        }
    }
}
