using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Menu;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    public sealed class UIManager : SingletonBehavior<UIManager>
    {
        [SerializeField]
        private PlayerUI _playerUIPrefab;

        [CanBeNull]
        private PlayerUI _playerUI;

        [CanBeNull]
        public PlayerUI PlayerUI => _playerUI;

        [SerializeField]
        private Menu.Menu _pauseMenuPrefab;

        [CanBeNull]
        private Menu.Menu _pauseMenu;

        private GameObject _uiContainer;

        private readonly Dictionary<string, GameObject> _uiObjects = new Dictionary<string, GameObject>();

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

        public void InitializePlayerUI(UnityEngine.Camera camera)
        {
            Debug.Log("Initializing player UI...");

            _playerUI = InstantiateUIPrefab(_playerUIPrefab);
            if(null != _playerUI) {
                _playerUI.Initialize(camera);
            }
        }

        public void Shutdown()
        {
            if(null != _playerUI) {
                Destroy(_playerUI.gameObject);
            }
            _playerUI = null;

            if(null != _pauseMenu) {
                Destroy(_pauseMenu.gameObject);
            }
            _pauseMenu = null;
        }

#region UI Objects
        public void RegisterUIObject(string name, GameObject uiObject)
        {
            //Debug.Log($"Registering UI object {name}: {uiObject.name}");
            _uiObjects.Add(name, uiObject);
        }

        public void UnregisterUIObject(string name)
        {
            //Debug.Log($"Unregistering UI object {name}");
            _uiObjects.Remove(name);
        }

        public void ShowUIObject(string name, bool show)
        {
            if(!_uiObjects.TryGetValue(name, out var uiObject)) {
                Debug.LogWarning($"Failed to lookup UI object {name}!");
                return;
            }

            //Debug.Log($"Showing UI object {name}: {show}");
            uiObject.SetActive(show);
        }
#endregion

        // helper for instantiating UI prefabs under the UI comtainer
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
                InputManager.Instance.EventSystem.UIModule.EnableAllActions();
            } else {
                InputManager.Instance.EventSystem.UIModule.DisableAllActions();
                _pauseMenu.gameObject.SetActive(false);
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.UIManager");
            debugMenuNode.RenderContentsAction = () => {
// TODO: print the known UI objects
            };
        }
    }
}
