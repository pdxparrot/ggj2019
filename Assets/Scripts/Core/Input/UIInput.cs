// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/UIInput.inputactions'

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;


namespace pdxpartyparrot.Core.Input
{
    [Serializable]
    public class UIInput : InputActionAssetReference
    {
        public UIInput()
        {
        }
        public UIInput(InputActionAsset asset)
            : base(asset)
        {
        }
        private bool m_Initialized;
        private void Initialize()
        {
            // ui
            m_ui = asset.GetActionMap("ui");
            m_ui_submit = m_ui.GetAction("submit");
            if (m_uiSubmitActionStarted != null)
                m_ui_submit.started += m_uiSubmitActionStarted.Invoke;
            if (m_uiSubmitActionPerformed != null)
                m_ui_submit.performed += m_uiSubmitActionPerformed.Invoke;
            if (m_uiSubmitActionCancelled != null)
                m_ui_submit.cancelled += m_uiSubmitActionCancelled.Invoke;
            m_ui_cancel = m_ui.GetAction("cancel");
            if (m_uiCancelActionStarted != null)
                m_ui_cancel.started += m_uiCancelActionStarted.Invoke;
            if (m_uiCancelActionPerformed != null)
                m_ui_cancel.performed += m_uiCancelActionPerformed.Invoke;
            if (m_uiCancelActionCancelled != null)
                m_ui_cancel.cancelled += m_uiCancelActionCancelled.Invoke;
            m_ui_navigate = m_ui.GetAction("navigate");
            if (m_uiNavigateActionStarted != null)
                m_ui_navigate.started += m_uiNavigateActionStarted.Invoke;
            if (m_uiNavigateActionPerformed != null)
                m_ui_navigate.performed += m_uiNavigateActionPerformed.Invoke;
            if (m_uiNavigateActionCancelled != null)
                m_ui_navigate.cancelled += m_uiNavigateActionCancelled.Invoke;
            m_ui_point = m_ui.GetAction("point");
            if (m_uiPointActionStarted != null)
                m_ui_point.started += m_uiPointActionStarted.Invoke;
            if (m_uiPointActionPerformed != null)
                m_ui_point.performed += m_uiPointActionPerformed.Invoke;
            if (m_uiPointActionCancelled != null)
                m_ui_point.cancelled += m_uiPointActionCancelled.Invoke;
            m_ui_leftclick = m_ui.GetAction("left click");
            if (m_uiLeftclickActionStarted != null)
                m_ui_leftclick.started += m_uiLeftclickActionStarted.Invoke;
            if (m_uiLeftclickActionPerformed != null)
                m_ui_leftclick.performed += m_uiLeftclickActionPerformed.Invoke;
            if (m_uiLeftclickActionCancelled != null)
                m_ui_leftclick.cancelled += m_uiLeftclickActionCancelled.Invoke;
            m_ui_rightclick = m_ui.GetAction("right click");
            if (m_uiRightclickActionStarted != null)
                m_ui_rightclick.started += m_uiRightclickActionStarted.Invoke;
            if (m_uiRightclickActionPerformed != null)
                m_ui_rightclick.performed += m_uiRightclickActionPerformed.Invoke;
            if (m_uiRightclickActionCancelled != null)
                m_ui_rightclick.cancelled += m_uiRightclickActionCancelled.Invoke;
            m_ui_middleclick = m_ui.GetAction("middle click");
            if (m_uiMiddleclickActionStarted != null)
                m_ui_middleclick.started += m_uiMiddleclickActionStarted.Invoke;
            if (m_uiMiddleclickActionPerformed != null)
                m_ui_middleclick.performed += m_uiMiddleclickActionPerformed.Invoke;
            if (m_uiMiddleclickActionCancelled != null)
                m_ui_middleclick.cancelled += m_uiMiddleclickActionCancelled.Invoke;
            m_Initialized = true;
        }
        private void Uninitialize()
        {
            m_ui = null;
            m_ui_submit = null;
            if (m_uiSubmitActionStarted != null)
                m_ui_submit.started -= m_uiSubmitActionStarted.Invoke;
            if (m_uiSubmitActionPerformed != null)
                m_ui_submit.performed -= m_uiSubmitActionPerformed.Invoke;
            if (m_uiSubmitActionCancelled != null)
                m_ui_submit.cancelled -= m_uiSubmitActionCancelled.Invoke;
            m_ui_cancel = null;
            if (m_uiCancelActionStarted != null)
                m_ui_cancel.started -= m_uiCancelActionStarted.Invoke;
            if (m_uiCancelActionPerformed != null)
                m_ui_cancel.performed -= m_uiCancelActionPerformed.Invoke;
            if (m_uiCancelActionCancelled != null)
                m_ui_cancel.cancelled -= m_uiCancelActionCancelled.Invoke;
            m_ui_navigate = null;
            if (m_uiNavigateActionStarted != null)
                m_ui_navigate.started -= m_uiNavigateActionStarted.Invoke;
            if (m_uiNavigateActionPerformed != null)
                m_ui_navigate.performed -= m_uiNavigateActionPerformed.Invoke;
            if (m_uiNavigateActionCancelled != null)
                m_ui_navigate.cancelled -= m_uiNavigateActionCancelled.Invoke;
            m_ui_point = null;
            if (m_uiPointActionStarted != null)
                m_ui_point.started -= m_uiPointActionStarted.Invoke;
            if (m_uiPointActionPerformed != null)
                m_ui_point.performed -= m_uiPointActionPerformed.Invoke;
            if (m_uiPointActionCancelled != null)
                m_ui_point.cancelled -= m_uiPointActionCancelled.Invoke;
            m_ui_leftclick = null;
            if (m_uiLeftclickActionStarted != null)
                m_ui_leftclick.started -= m_uiLeftclickActionStarted.Invoke;
            if (m_uiLeftclickActionPerformed != null)
                m_ui_leftclick.performed -= m_uiLeftclickActionPerformed.Invoke;
            if (m_uiLeftclickActionCancelled != null)
                m_ui_leftclick.cancelled -= m_uiLeftclickActionCancelled.Invoke;
            m_ui_rightclick = null;
            if (m_uiRightclickActionStarted != null)
                m_ui_rightclick.started -= m_uiRightclickActionStarted.Invoke;
            if (m_uiRightclickActionPerformed != null)
                m_ui_rightclick.performed -= m_uiRightclickActionPerformed.Invoke;
            if (m_uiRightclickActionCancelled != null)
                m_ui_rightclick.cancelled -= m_uiRightclickActionCancelled.Invoke;
            m_ui_middleclick = null;
            if (m_uiMiddleclickActionStarted != null)
                m_ui_middleclick.started -= m_uiMiddleclickActionStarted.Invoke;
            if (m_uiMiddleclickActionPerformed != null)
                m_ui_middleclick.performed -= m_uiMiddleclickActionPerformed.Invoke;
            if (m_uiMiddleclickActionCancelled != null)
                m_ui_middleclick.cancelled -= m_uiMiddleclickActionCancelled.Invoke;
            m_Initialized = false;
        }
        public void SetAsset(InputActionAsset newAsset)
        {
            if (newAsset == asset) return;
            if (m_Initialized) Uninitialize();
            asset = newAsset;
        }
        public override void MakePrivateCopyOfActions()
        {
            SetAsset(ScriptableObject.Instantiate(asset));
        }
        // ui
        private InputActionMap m_ui;
        private InputAction m_ui_submit;
        [SerializeField] private ActionEvent m_uiSubmitActionStarted;
        [SerializeField] private ActionEvent m_uiSubmitActionPerformed;
        [SerializeField] private ActionEvent m_uiSubmitActionCancelled;
        private InputAction m_ui_cancel;
        [SerializeField] private ActionEvent m_uiCancelActionStarted;
        [SerializeField] private ActionEvent m_uiCancelActionPerformed;
        [SerializeField] private ActionEvent m_uiCancelActionCancelled;
        private InputAction m_ui_navigate;
        [SerializeField] private ActionEvent m_uiNavigateActionStarted;
        [SerializeField] private ActionEvent m_uiNavigateActionPerformed;
        [SerializeField] private ActionEvent m_uiNavigateActionCancelled;
        private InputAction m_ui_point;
        [SerializeField] private ActionEvent m_uiPointActionStarted;
        [SerializeField] private ActionEvent m_uiPointActionPerformed;
        [SerializeField] private ActionEvent m_uiPointActionCancelled;
        private InputAction m_ui_leftclick;
        [SerializeField] private ActionEvent m_uiLeftclickActionStarted;
        [SerializeField] private ActionEvent m_uiLeftclickActionPerformed;
        [SerializeField] private ActionEvent m_uiLeftclickActionCancelled;
        private InputAction m_ui_rightclick;
        [SerializeField] private ActionEvent m_uiRightclickActionStarted;
        [SerializeField] private ActionEvent m_uiRightclickActionPerformed;
        [SerializeField] private ActionEvent m_uiRightclickActionCancelled;
        private InputAction m_ui_middleclick;
        [SerializeField] private ActionEvent m_uiMiddleclickActionStarted;
        [SerializeField] private ActionEvent m_uiMiddleclickActionPerformed;
        [SerializeField] private ActionEvent m_uiMiddleclickActionCancelled;
        public struct UiActions
        {
            private UIInput m_Wrapper;
            public UiActions(UIInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @submit { get { return m_Wrapper.m_ui_submit; } }
            public ActionEvent submitStarted { get { return m_Wrapper.m_uiSubmitActionStarted; } }
            public ActionEvent submitPerformed { get { return m_Wrapper.m_uiSubmitActionPerformed; } }
            public ActionEvent submitCancelled { get { return m_Wrapper.m_uiSubmitActionCancelled; } }
            public InputAction @cancel { get { return m_Wrapper.m_ui_cancel; } }
            public ActionEvent cancelStarted { get { return m_Wrapper.m_uiCancelActionStarted; } }
            public ActionEvent cancelPerformed { get { return m_Wrapper.m_uiCancelActionPerformed; } }
            public ActionEvent cancelCancelled { get { return m_Wrapper.m_uiCancelActionCancelled; } }
            public InputAction @navigate { get { return m_Wrapper.m_ui_navigate; } }
            public ActionEvent navigateStarted { get { return m_Wrapper.m_uiNavigateActionStarted; } }
            public ActionEvent navigatePerformed { get { return m_Wrapper.m_uiNavigateActionPerformed; } }
            public ActionEvent navigateCancelled { get { return m_Wrapper.m_uiNavigateActionCancelled; } }
            public InputAction @point { get { return m_Wrapper.m_ui_point; } }
            public ActionEvent pointStarted { get { return m_Wrapper.m_uiPointActionStarted; } }
            public ActionEvent pointPerformed { get { return m_Wrapper.m_uiPointActionPerformed; } }
            public ActionEvent pointCancelled { get { return m_Wrapper.m_uiPointActionCancelled; } }
            public InputAction @leftclick { get { return m_Wrapper.m_ui_leftclick; } }
            public ActionEvent leftclickStarted { get { return m_Wrapper.m_uiLeftclickActionStarted; } }
            public ActionEvent leftclickPerformed { get { return m_Wrapper.m_uiLeftclickActionPerformed; } }
            public ActionEvent leftclickCancelled { get { return m_Wrapper.m_uiLeftclickActionCancelled; } }
            public InputAction @rightclick { get { return m_Wrapper.m_ui_rightclick; } }
            public ActionEvent rightclickStarted { get { return m_Wrapper.m_uiRightclickActionStarted; } }
            public ActionEvent rightclickPerformed { get { return m_Wrapper.m_uiRightclickActionPerformed; } }
            public ActionEvent rightclickCancelled { get { return m_Wrapper.m_uiRightclickActionCancelled; } }
            public InputAction @middleclick { get { return m_Wrapper.m_ui_middleclick; } }
            public ActionEvent middleclickStarted { get { return m_Wrapper.m_uiMiddleclickActionStarted; } }
            public ActionEvent middleclickPerformed { get { return m_Wrapper.m_uiMiddleclickActionPerformed; } }
            public ActionEvent middleclickCancelled { get { return m_Wrapper.m_uiMiddleclickActionCancelled; } }
            public InputActionMap Get() { return m_Wrapper.m_ui; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(UiActions set) { return set.Get(); }
        }
        public UiActions @ui
        {
            get
            {
                if (!m_Initialized) Initialize();
                return new UiActions(this);
            }
        }
        [Serializable]
        public class ActionEvent : UnityEvent<InputAction.CallbackContext>
        {
        }
    }
}
