// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/ServerSpectator.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace pdxpartyparrot.Game.Input
{
    public class ServerSpectatorControls : IInputActionCollection
    {
        private InputActionAsset asset;
        public ServerSpectatorControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""ServerSpectator"",
    ""maps"": [
        {
            ""name"": ""ServerSpectator"",
            ""id"": ""1681b34d-9556-4729-bf48-7fc6079996dc"",
            ""actions"": [
                {
                    ""name"": ""move forward"",
                    ""id"": ""c906fce0-1fa7-4721-bc5c-fdd745b3220d"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move backward"",
                    ""id"": ""b4899855-b309-4bd4-94a7-2b5341fbc775"",
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
                    ""id"": ""1c1b3cbe-4d7f-4448-92fe-ea91564e9ae6"",
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
                    ""id"": ""da70e428-8b6d-4756-bfc4-8f56b518cfd3"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""move up"",
                    ""id"": ""22baea98-a69a-4b2a-a1b6-ef5122658f18"",
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
                    ""id"": ""1ebdadea-885b-49a7-b01f-0d0d40ddd991"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""look"",
                    ""id"": ""46873143-1856-417e-952d-3b450dfb3b8d"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
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
                    ""id"": ""c333c19e-5ba4-4b7e-8ad0-0858dcb5ffad"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move forward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""cd88da66-a790-48a1-a070-183bc4fd29af"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move backward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""49556d81-82ca-4551-82ed-782e7763f397"",
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
                    ""id"": ""005436b5-d072-4907-a7d5-b84feebe6998"",
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
                    ""id"": ""a7d50103-87e5-4343-b32b-19ea9a150233"",
                    ""path"": ""<Keyboard>/space"",
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
                    ""id"": ""7983e4b6-f24f-4663-9156-e138a085d92d"",
                    ""path"": ""<Keyboard>/leftShift"",
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
                    ""id"": ""8ec930a1-0dc7-42ff-9011-20257ed5658a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // ServerSpectator
            m_ServerSpectator = asset.GetActionMap("ServerSpectator");
            m_ServerSpectator_moveforward = m_ServerSpectator.GetAction("move forward");
            m_ServerSpectator_movebackward = m_ServerSpectator.GetAction("move backward");
            m_ServerSpectator_moveleft = m_ServerSpectator.GetAction("move left");
            m_ServerSpectator_moveright = m_ServerSpectator.GetAction("move right");
            m_ServerSpectator_moveup = m_ServerSpectator.GetAction("move up");
            m_ServerSpectator_movedown = m_ServerSpectator.GetAction("move down");
            m_ServerSpectator_look = m_ServerSpectator.GetAction("look");
        }
        ~ServerSpectatorControls()
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
        // ServerSpectator
        private InputActionMap m_ServerSpectator;
        private IServerSpectatorActions m_ServerSpectatorActionsCallbackInterface;
        private InputAction m_ServerSpectator_moveforward;
        private InputAction m_ServerSpectator_movebackward;
        private InputAction m_ServerSpectator_moveleft;
        private InputAction m_ServerSpectator_moveright;
        private InputAction m_ServerSpectator_moveup;
        private InputAction m_ServerSpectator_movedown;
        private InputAction m_ServerSpectator_look;
        public struct ServerSpectatorActions
        {
            private ServerSpectatorControls m_Wrapper;
            public ServerSpectatorActions(ServerSpectatorControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @moveforward { get { return m_Wrapper.m_ServerSpectator_moveforward; } }
            public InputAction @movebackward { get { return m_Wrapper.m_ServerSpectator_movebackward; } }
            public InputAction @moveleft { get { return m_Wrapper.m_ServerSpectator_moveleft; } }
            public InputAction @moveright { get { return m_Wrapper.m_ServerSpectator_moveright; } }
            public InputAction @moveup { get { return m_Wrapper.m_ServerSpectator_moveup; } }
            public InputAction @movedown { get { return m_Wrapper.m_ServerSpectator_movedown; } }
            public InputAction @look { get { return m_Wrapper.m_ServerSpectator_look; } }
            public InputActionMap Get() { return m_Wrapper.m_ServerSpectator; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(ServerSpectatorActions set) { return set.Get(); }
            public void SetCallbacks(IServerSpectatorActions instance)
            {
                if (m_Wrapper.m_ServerSpectatorActionsCallbackInterface != null)
                {
                    moveforward.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveforward;
                    moveforward.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveforward;
                    moveforward.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveforward;
                    movebackward.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovebackward;
                    movebackward.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovebackward;
                    movebackward.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovebackward;
                    moveleft.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveleft;
                    moveleft.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveleft;
                    moveleft.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveleft;
                    moveright.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveright;
                    moveright.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveright;
                    moveright.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveright;
                    moveup.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveup;
                    moveup.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveup;
                    moveup.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMoveup;
                    movedown.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovedown;
                    movedown.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovedown;
                    movedown.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnMovedown;
                    look.started -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnLook;
                    look.performed -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnLook;
                    look.cancelled -= m_Wrapper.m_ServerSpectatorActionsCallbackInterface.OnLook;
                }
                m_Wrapper.m_ServerSpectatorActionsCallbackInterface = instance;
                if (instance != null)
                {
                    moveforward.started += instance.OnMoveforward;
                    moveforward.performed += instance.OnMoveforward;
                    moveforward.cancelled += instance.OnMoveforward;
                    movebackward.started += instance.OnMovebackward;
                    movebackward.performed += instance.OnMovebackward;
                    movebackward.cancelled += instance.OnMovebackward;
                    moveleft.started += instance.OnMoveleft;
                    moveleft.performed += instance.OnMoveleft;
                    moveleft.cancelled += instance.OnMoveleft;
                    moveright.started += instance.OnMoveright;
                    moveright.performed += instance.OnMoveright;
                    moveright.cancelled += instance.OnMoveright;
                    moveup.started += instance.OnMoveup;
                    moveup.performed += instance.OnMoveup;
                    moveup.cancelled += instance.OnMoveup;
                    movedown.started += instance.OnMovedown;
                    movedown.performed += instance.OnMovedown;
                    movedown.cancelled += instance.OnMovedown;
                    look.started += instance.OnLook;
                    look.performed += instance.OnLook;
                    look.cancelled += instance.OnLook;
                }
            }
        }
        public ServerSpectatorActions @ServerSpectator
        {
            get
            {
                return new ServerSpectatorActions(this);
            }
        }
        public interface IServerSpectatorActions
        {
            void OnMoveforward(InputAction.CallbackContext context);
            void OnMovebackward(InputAction.CallbackContext context);
            void OnMoveleft(InputAction.CallbackContext context);
            void OnMoveright(InputAction.CallbackContext context);
            void OnMoveup(InputAction.CallbackContext context);
            void OnMovedown(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
        }
    }
}
