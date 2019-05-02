// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Player2d.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace pdxpartyparrot.ggj2019.Input
{
    public class PlayerControls : IInputActionCollection
    {
        private InputActionAsset asset;
        public PlayerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player2d"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""eb0fa0dc-a089-4476-beeb-f045a8d5bdb8"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""id"": ""28e9668b-c2ab-4765-9c01-0e223f59a602"",
                    ""expectedControlLayout"": ""Stick"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Pause"",
                    ""id"": ""cd3f0036-5277-4d0a-8437-1fc31195d39c"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move up"",
                    ""id"": ""5a814028-acec-4506-a2f0-80a948e20861"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move down"",
                    ""id"": ""68e65cea-0ad5-44fe-976e-c615d0c3ca38"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move left"",
                    ""id"": ""5e473936-00f1-461c-8a3b-d25e014fcf10"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move right"",
                    ""id"": ""728c72d4-35c0-40a8-ad7a-a063b63d9ed2"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""gather"",
                    ""id"": ""35df6f66-304c-434d-8bf4-c790b692e261"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""context"",
                    ""id"": ""bb0c3b7e-2f68-4586-84c5-b2442ffcde2a"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""movedpad"",
                    ""id"": ""9cc5316b-1a69-4dc1-966f-f2bb1164e765"",
                    ""expectedControlLayout"": ""Dpad"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""700dfbcd-2961-476b-a783-aee4af4b6d05"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""afb62351-903f-4d97-a8cc-695cc062debc"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""8e4db294-84fe-4e9a-87ff-055aba0cffee"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""a3440d9f-c72f-496b-8965-f16f8faca119"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""5c9c5fe8-ceae-409d-b614-2d6b52c4873d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""434e2c5b-f802-4570-8935-b2c315d6ae64"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""356920fb-2099-42a5-a234-bdc66d94b0af"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""a519bfdb-17af-43fe-9b95-c96b49bca087"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gather"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""f5a6ed3f-7b04-4081-95ad-f9089453f2c6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gather"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""b4f5f20f-8c81-45b6-aeba-07430a5b6500"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""gather"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""055217d8-64f0-4554-a0f2-a1332224545d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""context"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9f536365-f61f-42e6-9440-b2bd634f3899"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""context"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""c39badd2-8e07-468b-b9eb-5c8a142e99bc"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""context"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""5a0a58f3-c075-4e06-bd11-60631e3f642e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""context"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""d8329d3b-1874-45bb-8971-ec6188f1ff9c"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movedpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.GetActionMap("Player");
            m_Player_move = m_Player.GetAction("move");
            m_Player_Pause = m_Player.GetAction("Pause");
            m_Player_moveup = m_Player.GetAction("move up");
            m_Player_movedown = m_Player.GetAction("move down");
            m_Player_moveleft = m_Player.GetAction("move left");
            m_Player_moveright = m_Player.GetAction("move right");
            m_Player_gather = m_Player.GetAction("gather");
            m_Player_context = m_Player.GetAction("context");
            m_Player_movedpad = m_Player.GetAction("movedpad");
        }
        ~PlayerControls()
        {
            UnityEngine.Object.Destroy(asset);
        }
        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }
        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }
        public ReadOnlyArray<InputControlScheme> controlSchemes
        {
            get => asset.controlSchemes;
        }
        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }
        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Enable()
        {
            asset.Enable();
        }
        public void Disable()
        {
            asset.Disable();
        }
        // Player
        private InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private InputAction m_Player_move;
        private InputAction m_Player_Pause;
        private InputAction m_Player_moveup;
        private InputAction m_Player_movedown;
        private InputAction m_Player_moveleft;
        private InputAction m_Player_moveright;
        private InputAction m_Player_gather;
        private InputAction m_Player_context;
        private InputAction m_Player_movedpad;
        public struct PlayerActions
        {
            private PlayerControls m_Wrapper;
            public PlayerActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @move { get { return m_Wrapper.m_Player_move; } }
            public InputAction @Pause { get { return m_Wrapper.m_Player_Pause; } }
            public InputAction @moveup { get { return m_Wrapper.m_Player_moveup; } }
            public InputAction @movedown { get { return m_Wrapper.m_Player_movedown; } }
            public InputAction @moveleft { get { return m_Wrapper.m_Player_moveleft; } }
            public InputAction @moveright { get { return m_Wrapper.m_Player_moveright; } }
            public InputAction @gather { get { return m_Wrapper.m_Player_gather; } }
            public InputAction @context { get { return m_Wrapper.m_Player_context; } }
            public InputAction @movedpad { get { return m_Wrapper.m_Player_movedpad; } }
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    move.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    Pause.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    moveup.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveup;
                    moveup.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveup;
                    moveup.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveup;
                    movedown.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedown;
                    movedown.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedown;
                    movedown.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedown;
                    moveleft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveleft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveleft.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveright.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                    moveright.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                    moveright.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                    gather.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGather;
                    gather.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGather;
                    gather.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGather;
                    context.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnContext;
                    context.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnContext;
                    context.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnContext;
                    movedpad.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedpad;
                    movedpad.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedpad;
                    movedpad.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovedpad;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    move.started += instance.OnMove;
                    move.performed += instance.OnMove;
                    move.cancelled += instance.OnMove;
                    Pause.started += instance.OnPause;
                    Pause.performed += instance.OnPause;
                    Pause.cancelled += instance.OnPause;
                    moveup.started += instance.OnMoveup;
                    moveup.performed += instance.OnMoveup;
                    moveup.cancelled += instance.OnMoveup;
                    movedown.started += instance.OnMovedown;
                    movedown.performed += instance.OnMovedown;
                    movedown.cancelled += instance.OnMovedown;
                    moveleft.started += instance.OnMoveleft;
                    moveleft.performed += instance.OnMoveleft;
                    moveleft.cancelled += instance.OnMoveleft;
                    moveright.started += instance.OnMoveright;
                    moveright.performed += instance.OnMoveright;
                    moveright.cancelled += instance.OnMoveright;
                    gather.started += instance.OnGather;
                    gather.performed += instance.OnGather;
                    gather.cancelled += instance.OnGather;
                    context.started += instance.OnContext;
                    context.performed += instance.OnContext;
                    context.cancelled += instance.OnContext;
                    movedpad.started += instance.OnMovedpad;
                    movedpad.performed += instance.OnMovedpad;
                    movedpad.cancelled += instance.OnMovedpad;
                }
            }
        }
        public PlayerActions @Player
        {
            get
            {
                return new PlayerActions(this);
            }
        }
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
            void OnMoveup(InputAction.CallbackContext context);
            void OnMovedown(InputAction.CallbackContext context);
            void OnMoveleft(InputAction.CallbackContext context);
            void OnMoveright(InputAction.CallbackContext context);
            void OnGather(InputAction.CallbackContext context);
            void OnContext(InputAction.CallbackContext context);
            void OnMovedpad(InputAction.CallbackContext context);
        }
    }
}
