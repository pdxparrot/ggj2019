using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Menu;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    public sealed class UIManager : SingletonBehavior<UIManager>
    {
        private struct FloatingTextEntry
        {
            public string poolName;

            public string text;

            public Color color;

            public Func<Vector3> position;
        }

#region UI / Menus
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
#endregion

#region Floating Text
        [SerializeField]
        private string _defaultFloatingTextPoolName = "floating_text";

        public string DefaultFloatingTextPoolName => _defaultFloatingTextPoolName;

        [SerializeField]
        private float _floatingTextSpawnRate = 0.1f;

        [SerializeField]
        private float _floatingTextSpawnOffset = 1.0f;

        [SerializeField]
        [ReadOnly]
        private float _nextFloatingTextSpawnOffset;
#endregion

        private GameObject _uiContainer;
        private GameObject _floatingTextContainer;

        private readonly Dictionary<string, UIObject> _uiObjects = new Dictionary<string, UIObject>();

        private readonly Queue<FloatingTextEntry> _floatingText = new Queue<FloatingTextEntry>();

        private Coroutine _floatingTextRoutine;

#region Unity Lifecycle
        private void Awake()
        {
            _uiContainer = new GameObject("UI");
            _floatingTextContainer = new GameObject("Floating Text");

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }

            Destroy(_floatingTextContainer);
            _floatingTextContainer = null;

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

            _floatingTextRoutine = StartCoroutine(FloatingTextRoutine());
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
            if(null != _floatingTextRoutine) {
                StopCoroutine(_floatingTextRoutine);
            }
            _floatingTextRoutine = null;

            _floatingText.Clear();

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
        public void RegisterUIObject(UIObject uiObject)
        {
            //Debug.Log($"Registering UI object {uiObject.Id}: {uiObject.name}");
            try {
                _uiObjects.Add(uiObject.Id, uiObject);
            } catch(ArgumentException) {
                Debug.LogWarning($"Failed overwrite of UI object {uiObject.Id}!");
            }
        }

        public bool UnregisterUIObject(UIObject uiObject)
        {
            //Debug.Log($"Unregistering UI object {uiObject.Id}");
            return _uiObjects.Remove(uiObject.Id);
        }

        public void ShowUIObject(string id, bool show)
        {
            if(!_uiObjects.TryGetValue(id, out var uiObject)) {
                Debug.LogWarning($"Failed to lookup UI object {id}!");
                return;
            }

            //Debug.Log($"Showing UI object {name}: {show}");
            uiObject.gameObject.SetActive(show);
        }
#endregion

        // helper for instantiating UI prefabs under the UI comtainer
        public TV InstantiateUIPrefab<TV>(TV prefab) where TV: Component
        {
            return Instantiate(prefab, _uiContainer.transform);
        }

#region Floating Text
        public void QueueFloatingText(string poolName, string text, Color color, Func<Vector3> position)
        {
            _floatingText.Enqueue(new FloatingTextEntry
            {
                poolName = poolName,
                text = text,
                color = color,
                position = position
            });
        }
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(null == _pauseMenu) {
                return;
            }

            if(PartyParrotManager.Instance.IsPaused) {
                _pauseMenu.gameObject.SetActive(true);
                _pauseMenu.ResetMenu();

                if(InputManager.Instance.DebugInput) {
                    Debug.Log("Enabling UIModule actions");
                }
                InputManager.Instance.EventSystem.UIModule.EnableAllActions();
            } else {
                if(InputManager.Instance.DebugInput) {
                    Debug.Log("Disabling UIModule actions");
                }
                InputManager.Instance.EventSystem.UIModule.DisableAllActions();

                _pauseMenu.gameObject.SetActive(false);
            }
        }
#endregion

        private IEnumerator FloatingTextRoutine()
        {
            // start at an extremity
            _nextFloatingTextSpawnOffset = _floatingTextSpawnOffset;

            WaitForSeconds wait = new WaitForSeconds(_floatingTextSpawnRate);
            while(true) {
                yield return wait;

                if(_floatingText.Count < 1) {
                    continue;
                }

                FloatingTextEntry entry = _floatingText.Dequeue();

                FloatingText floatingText = ObjectPoolManager.Instance.GetPooledObject<FloatingText>(entry.poolName, _floatingTextContainer.transform);
                if(null == floatingText) {
                    Debug.LogWarning($"Failed to get floating text from pool {entry.poolName}!");
                    continue;
                }

                // offset our starting x (TODO: offset z also ?)
                Vector3 position = entry.position();
                position.x += _nextFloatingTextSpawnOffset;
                _nextFloatingTextSpawnOffset = -_nextFloatingTextSpawnOffset;

                floatingText.Text.text = entry.text;
                floatingText.Text.color = entry.color;
                floatingText.Show(position);
            }
        }

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.UIManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("UI Objects:", GUI.skin.box);
                    foreach(var kvp in _uiObjects) {
                        GUILayout.Label(kvp.Key);
                    }
                GUILayout.EndVertical();

                GUILayout.Label($"Queued floating text: {_floatingText.Count}");
                if(GUIUtils.LayoutButton("Spawn Floating Text")) {
                    // TODO: need to be able to input the params to this
                    QueueFloatingText(DefaultFloatingTextPoolName, "Test", Color.black, () => Vector3.zero);
                }
            };
        }
    }
}
