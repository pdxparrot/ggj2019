using System;
using System.Collections.Generic;
using System.Linq;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Core.Input
{
    // TODO: InputSystem is still fleshing out multiple controller support
    // so this will need an update once that's done
    public sealed class InputManager : SingletonBehavior<InputManager>
    {
        // config keys
        private const string EnableVibrationKey = "input.vibration.enable";

        private static int _lastListenerId;

        private static int NextListenerId => ++_lastListenerId;

        private class GamepadListener
        {
            public int id;

            public Action<Gamepad> acquireCallback;

            public Action<Gamepad> disconnectCallback;
        }

        [SerializeField]
        private EventSystemHelper _eventSystemPrefab;

        public EventSystemHelper EventSystem { get; private set; }

        public int GamepadCount => _unacquiredGamepads.Count + _acquiredGamepads.Count;

        public bool EnableVibration
        {
            get => PartyParrotManager.Instance.GetBool(EnableVibrationKey, true);
            set => PartyParrotManager.Instance.SetBool(EnableVibrationKey, value);
        }

#region Gamepads
        private readonly List<Gamepad> _unacquiredGamepads = new List<Gamepad>();

        private readonly Dictionary<Gamepad, GamepadListener> _acquiredGamepads = new Dictionary<Gamepad, GamepadListener>();

        private readonly List<GamepadListener> _gamepadListeners = new List<GamepadListener>();
#endregion

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();

            bool inputInitialized = false;

#if ENABLE_VR
            if(!inputInitialized && PartyParrotManager.Instance.EnableVR) {
                Debug.LogError("TODO: Handle VR Input");
                inputInitialized = true;
            }
#endif

#if ENABLE_GVR
            if(!inputInitialized && PartyParrotManager.Instance.EnableGoogleVR) {
                Debug.LogError("TODO: Handle Google VR Input");
                inputInitialized = true;
            }
#endif

            if(!inputInitialized) {
                Debug.Log("Creating EventSystem...");
                EventSystem = Instantiate(_eventSystemPrefab, transform);
            }

            InitGamepads();

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        protected override void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;

            Destroy(EventSystem.gameObject);
            EventSystem = null;

            base.OnDestroy();
        }
#endregion

#region Gamepads
        public int AcquireGamepad(Action<Gamepad> acquireCallback, Action<Gamepad> disconnectCallback)
        {
            GamepadListener listener = new GamepadListener
            {
                id = NextListenerId,
                acquireCallback = acquireCallback,
                disconnectCallback = disconnectCallback
            };

            AcquireGamepad(listener);

            return listener.id;
        }

        private void AcquireGamepad(GamepadListener listener)
        {
            if(_unacquiredGamepads.Count < 1) {
                _gamepadListeners.Add(listener);
                return;
            }

            Gamepad gamepad = _unacquiredGamepads.RemoveFront();
            AcquireGamepad(listener, gamepad);
        }

        private void AcquireGamepad(GamepadListener listener, Gamepad gamepad)
        {
            Debug.Log($"Gamepad listener {listener.id} acquiring gamepad {gamepad.name}");

            listener.acquireCallback.Invoke(gamepad);
            _acquiredGamepads[gamepad] = listener;
        }

        public void ReleaseGamepad(int listenerId)
        {
            if(listenerId < 1) {
                return;
            }

            _gamepadListeners.RemoveAll(x => x.id == listenerId);

            List<Gamepad> remove = new List<Gamepad>();
            foreach(var kvp in _acquiredGamepads) {
                if(kvp.Value.id == listenerId) {
                    remove.Add(kvp.Key);
                }
            }

            foreach(Gamepad gamepad in remove) {
                Debug.Log($"Gamepad listener {listenerId} releasing gamepad {gamepad.name}");

                _acquiredGamepads.Remove(gamepad);
                _unacquiredGamepads.Add(gamepad);
            }
        }

        private void InitGamepads()
        {
            var gamepads = from device in InputSystem.devices where device is Gamepad select (Gamepad)device;
            foreach(Gamepad gamepad in gamepads) {
                _unacquiredGamepads.Add(gamepad);
            }
            Debug.Log($"Found {_unacquiredGamepads.Count} gamepads");
        }

        private void AddGamepad(Gamepad gamepad)
        {
            Debug.Log("Gamepad added");

            if(!NotifyAddGamepad(gamepad)) {
                _unacquiredGamepads.Add(gamepad);
            }
        }

        private void RemoveGamepad(Gamepad gamepad)
        {
            Debug.Log("Gamepad removed");

            if(_unacquiredGamepads.Remove(gamepad)) {
                return;
            }

            NotifyRemoveGamepad(gamepad);
        }

        private bool NotifyAddGamepad(Gamepad gamepad)
        {
            if(_gamepadListeners.Count < 1) {
                return false;
            }

            GamepadListener listener = _gamepadListeners.RemoveFront();
            AcquireGamepad(listener, gamepad);

            return true;
        }

        private void NotifyRemoveGamepad(Gamepad gamepad)
        {
            Debug.Log($"Gamepad {gamepad.name} is offline");

            GamepadListener listener = _acquiredGamepads.GetOrDefault(gamepad);
            listener?.disconnectCallback.Invoke(gamepad);
            _acquiredGamepads.Remove(gamepad);
            _gamepadListeners.Add(listener);
        }
#endregion

#region Event Handlers
        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if(device is Gamepad) {
                OnGamepadChange((Gamepad)device, change);
                return;
            }
        }

        private void OnGamepadChange(Gamepad gamepad, InputDeviceChange change)
        {
            switch (change)
            {
            case InputDeviceChange.Added:
                AddGamepad(gamepad);
                break;
            case InputDeviceChange.Enabled:
                Debug.LogWarning("Unhandled gamepad enabled");
                break;
            case InputDeviceChange.Removed:
                RemoveGamepad(gamepad);
                break;
            case InputDeviceChange.Disabled:
                Debug.LogWarning("Unhandled gamepad disabled");
                break;
            case InputDeviceChange.StateChanged:
                break;
            default:
                Debug.LogWarning($"Unhandled gamepad change: {change}");
                break;
            }
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.InputManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Gamepads", GUI.skin.box);
                    EnableVibration = GUILayout.Toggle(EnableVibration, "Enable Vibration");

                    GUILayout.Label($"Queued listeners: {_gamepadListeners.Count}");

                    GUILayout.BeginVertical("Unacquired:", GUI.skin.box);
                        foreach(Gamepad gamepad in _unacquiredGamepads) {
                            GUILayout.Label(gamepad.name);
                        }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Acquired:", GUI.skin.box);
                        foreach(var kvp in _acquiredGamepads) {
                            GUILayout.Label($"{kvp.Key.name}:{kvp.Value.id}");
                        }
                    GUILayout.EndVertical();
                GUILayout.EndVertical();
            };
        }
    }
}

