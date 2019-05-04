// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/UIInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace pdxpartyparrot.Core.Input
{
    public class UIInput : IInputActionCollection
    {
        private InputActionAsset asset;
        public UIInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""UIInput"",
    ""maps"": [
        {
            ""name"": ""ui"",
            ""id"": ""ee67acf9-1850-473d-a6c7-541a3b1138c3"",
            ""actions"": [
                {
                    ""name"": ""submit"",
                    ""id"": ""ec8acc79-062b-4466-8d97-db02f8fa133d"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""cancel"",
                    ""id"": ""f9b62ac2-ca93-4df5-9011-bc0fd05cfd93"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""navigate"",
                    ""id"": ""0269d4b6-6da5-41c7-83b3-d2bbcfe0a0e6"",
                    ""expectedControlLayout"": ""Dpad"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""point"",
                    ""id"": ""4c97b087-9d25-4049-9826-2ad4364f0fab"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""left click"",
                    ""id"": ""b39b41b3-a87e-4ac4-b9b5-9456252c3e82"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""right click"",
                    ""id"": ""8d4c2b75-540d-47ec-bad4-e6e44d5d71f2"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                },
                {
                    ""name"": ""middle click"",
                    ""id"": ""ae9ff2fa-1de8-4e9d-8ec1-eb9ec8204068"",
                    ""expectedControlLayout"": ""Button"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5ffe388f-21b0-4abc-a800-2e73fcd273c5"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""0f5d74eb-ad80-4bb5-981a-6ea2cf65fc79"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""07efc56b-d123-4034-82f4-7922938f939e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""05a2ed26-2b85-4b6c-907c-4466e636ec2f"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""00a5e3f1-21f0-4c60-aefa-076ca568ee29"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""977fc740-2c79-4615-89cb-cb847d1d793d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""a9997c9c-a329-4b87-b7ee-12a729f6bdff"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""4905fc64-21b6-4a15-afc1-bc73c4046dcd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9d969b09-9d0d-4e12-8ed0-7958b5f0316b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""6fcb1bfd-3e0d-4f14-8a04-20b083c2bbd4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""right click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""16e2fd4c-ddec-4ee9-b11b-cd94e680173a"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""middle click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // ui
            m_ui = asset.GetActionMap("ui");
            m_ui_submit = m_ui.GetAction("submit");
            m_ui_cancel = m_ui.GetAction("cancel");
            m_ui_navigate = m_ui.GetAction("navigate");
            m_ui_point = m_ui.GetAction("point");
            m_ui_leftclick = m_ui.GetAction("left click");
            m_ui_rightclick = m_ui.GetAction("right click");
            m_ui_middleclick = m_ui.GetAction("middle click");
        }
        ~UIInput()
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
        // ui
        private InputActionMap m_ui;
        private IUiActions m_UiActionsCallbackInterface;
        private InputAction m_ui_submit;
        private InputAction m_ui_cancel;
        private InputAction m_ui_navigate;
        private InputAction m_ui_point;
        private InputAction m_ui_leftclick;
        private InputAction m_ui_rightclick;
        private InputAction m_ui_middleclick;
        public struct UiActions
        {
            private UIInput m_Wrapper;
            public UiActions(UIInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @submit { get { return m_Wrapper.m_ui_submit; } }
            public InputAction @cancel { get { return m_Wrapper.m_ui_cancel; } }
            public InputAction @navigate { get { return m_Wrapper.m_ui_navigate; } }
            public InputAction @point { get { return m_Wrapper.m_ui_point; } }
            public InputAction @leftclick { get { return m_Wrapper.m_ui_leftclick; } }
            public InputAction @rightclick { get { return m_Wrapper.m_ui_rightclick; } }
            public InputAction @middleclick { get { return m_Wrapper.m_ui_middleclick; } }
            public InputActionMap Get() { return m_Wrapper.m_ui; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(UiActions set) { return set.Get(); }
            public void SetCallbacks(IUiActions instance)
            {
                if (m_Wrapper.m_UiActionsCallbackInterface != null)
                {
                    submit.started -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                    submit.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                    submit.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                    cancel.started -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                    cancel.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                    cancel.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                    navigate.started -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                    navigate.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                    navigate.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                    point.started -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                    point.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                    point.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                    leftclick.started -= m_Wrapper.m_UiActionsCallbackInterface.OnLeftclick;
                    leftclick.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnLeftclick;
                    leftclick.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnLeftclick;
                    rightclick.started -= m_Wrapper.m_UiActionsCallbackInterface.OnRightclick;
                    rightclick.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnRightclick;
                    rightclick.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnRightclick;
                    middleclick.started -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleclick;
                    middleclick.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleclick;
                    middleclick.cancelled -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleclick;
                }
                m_Wrapper.m_UiActionsCallbackInterface = instance;
                if (instance != null)
                {
                    submit.started += instance.OnSubmit;
                    submit.performed += instance.OnSubmit;
                    submit.cancelled += instance.OnSubmit;
                    cancel.started += instance.OnCancel;
                    cancel.performed += instance.OnCancel;
                    cancel.cancelled += instance.OnCancel;
                    navigate.started += instance.OnNavigate;
                    navigate.performed += instance.OnNavigate;
                    navigate.cancelled += instance.OnNavigate;
                    point.started += instance.OnPoint;
                    point.performed += instance.OnPoint;
                    point.cancelled += instance.OnPoint;
                    leftclick.started += instance.OnLeftclick;
                    leftclick.performed += instance.OnLeftclick;
                    leftclick.cancelled += instance.OnLeftclick;
                    rightclick.started += instance.OnRightclick;
                    rightclick.performed += instance.OnRightclick;
                    rightclick.cancelled += instance.OnRightclick;
                    middleclick.started += instance.OnMiddleclick;
                    middleclick.performed += instance.OnMiddleclick;
                    middleclick.cancelled += instance.OnMiddleclick;
                }
            }
        }
        public UiActions @ui
        {
            get
            {
                return new UiActions(this);
            }
        }
        public interface IUiActions
        {
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnNavigate(InputAction.CallbackContext context);
            void OnPoint(InputAction.CallbackContext context);
            void OnLeftclick(InputAction.CallbackContext context);
            void OnRightclick(InputAction.CallbackContext context);
            void OnMiddleclick(InputAction.CallbackContext context);
        }
    }
}
