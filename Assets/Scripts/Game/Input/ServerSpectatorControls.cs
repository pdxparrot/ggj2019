// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/ServerSpectator.inputactions'

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;


namespace pdxpartyparrot.Game.Input
{
    [Serializable]
    public class ServerSpectatorControls : InputActionAssetReference
    {
        public ServerSpectatorControls()
        {
        }
        public ServerSpectatorControls(InputActionAsset asset)
            : base(asset)
        {
        }
        private bool m_Initialized;
        private void Initialize()
        {
            // ServerSpectator
            m_ServerSpectator = asset.GetActionMap("ServerSpectator");
            m_ServerSpectator_moveforward = m_ServerSpectator.GetAction("move forward");
            if (m_ServerSpectatorMoveforwardActionStarted != null)
                m_ServerSpectator_moveforward.started += m_ServerSpectatorMoveforwardActionStarted.Invoke;
            if (m_ServerSpectatorMoveforwardActionPerformed != null)
                m_ServerSpectator_moveforward.performed += m_ServerSpectatorMoveforwardActionPerformed.Invoke;
            if (m_ServerSpectatorMoveforwardActionCancelled != null)
                m_ServerSpectator_moveforward.cancelled += m_ServerSpectatorMoveforwardActionCancelled.Invoke;
            m_ServerSpectator_movebackward = m_ServerSpectator.GetAction("move backward");
            if (m_ServerSpectatorMovebackwardActionStarted != null)
                m_ServerSpectator_movebackward.started += m_ServerSpectatorMovebackwardActionStarted.Invoke;
            if (m_ServerSpectatorMovebackwardActionPerformed != null)
                m_ServerSpectator_movebackward.performed += m_ServerSpectatorMovebackwardActionPerformed.Invoke;
            if (m_ServerSpectatorMovebackwardActionCancelled != null)
                m_ServerSpectator_movebackward.cancelled += m_ServerSpectatorMovebackwardActionCancelled.Invoke;
            m_ServerSpectator_moveleft = m_ServerSpectator.GetAction("move left");
            if (m_ServerSpectatorMoveleftActionStarted != null)
                m_ServerSpectator_moveleft.started += m_ServerSpectatorMoveleftActionStarted.Invoke;
            if (m_ServerSpectatorMoveleftActionPerformed != null)
                m_ServerSpectator_moveleft.performed += m_ServerSpectatorMoveleftActionPerformed.Invoke;
            if (m_ServerSpectatorMoveleftActionCancelled != null)
                m_ServerSpectator_moveleft.cancelled += m_ServerSpectatorMoveleftActionCancelled.Invoke;
            m_ServerSpectator_moveright = m_ServerSpectator.GetAction("move right");
            if (m_ServerSpectatorMoverightActionStarted != null)
                m_ServerSpectator_moveright.started += m_ServerSpectatorMoverightActionStarted.Invoke;
            if (m_ServerSpectatorMoverightActionPerformed != null)
                m_ServerSpectator_moveright.performed += m_ServerSpectatorMoverightActionPerformed.Invoke;
            if (m_ServerSpectatorMoverightActionCancelled != null)
                m_ServerSpectator_moveright.cancelled += m_ServerSpectatorMoverightActionCancelled.Invoke;
            m_ServerSpectator_moveup = m_ServerSpectator.GetAction("move up");
            if (m_ServerSpectatorMoveupActionStarted != null)
                m_ServerSpectator_moveup.started += m_ServerSpectatorMoveupActionStarted.Invoke;
            if (m_ServerSpectatorMoveupActionPerformed != null)
                m_ServerSpectator_moveup.performed += m_ServerSpectatorMoveupActionPerformed.Invoke;
            if (m_ServerSpectatorMoveupActionCancelled != null)
                m_ServerSpectator_moveup.cancelled += m_ServerSpectatorMoveupActionCancelled.Invoke;
            m_ServerSpectator_movedown = m_ServerSpectator.GetAction("move down");
            if (m_ServerSpectatorMovedownActionStarted != null)
                m_ServerSpectator_movedown.started += m_ServerSpectatorMovedownActionStarted.Invoke;
            if (m_ServerSpectatorMovedownActionPerformed != null)
                m_ServerSpectator_movedown.performed += m_ServerSpectatorMovedownActionPerformed.Invoke;
            if (m_ServerSpectatorMovedownActionCancelled != null)
                m_ServerSpectator_movedown.cancelled += m_ServerSpectatorMovedownActionCancelled.Invoke;
            m_ServerSpectator_look = m_ServerSpectator.GetAction("look");
            if (m_ServerSpectatorLookActionStarted != null)
                m_ServerSpectator_look.started += m_ServerSpectatorLookActionStarted.Invoke;
            if (m_ServerSpectatorLookActionPerformed != null)
                m_ServerSpectator_look.performed += m_ServerSpectatorLookActionPerformed.Invoke;
            if (m_ServerSpectatorLookActionCancelled != null)
                m_ServerSpectator_look.cancelled += m_ServerSpectatorLookActionCancelled.Invoke;
            m_Initialized = true;
        }
        private void Uninitialize()
        {
            if (m_ServerSpectatorActionsCallbackInterface != null)
            {
                ServerSpectator.SetCallbacks(null);
            }
            m_ServerSpectator = null;
            m_ServerSpectator_moveforward = null;
            if (m_ServerSpectatorMoveforwardActionStarted != null)
                m_ServerSpectator_moveforward.started -= m_ServerSpectatorMoveforwardActionStarted.Invoke;
            if (m_ServerSpectatorMoveforwardActionPerformed != null)
                m_ServerSpectator_moveforward.performed -= m_ServerSpectatorMoveforwardActionPerformed.Invoke;
            if (m_ServerSpectatorMoveforwardActionCancelled != null)
                m_ServerSpectator_moveforward.cancelled -= m_ServerSpectatorMoveforwardActionCancelled.Invoke;
            m_ServerSpectator_movebackward = null;
            if (m_ServerSpectatorMovebackwardActionStarted != null)
                m_ServerSpectator_movebackward.started -= m_ServerSpectatorMovebackwardActionStarted.Invoke;
            if (m_ServerSpectatorMovebackwardActionPerformed != null)
                m_ServerSpectator_movebackward.performed -= m_ServerSpectatorMovebackwardActionPerformed.Invoke;
            if (m_ServerSpectatorMovebackwardActionCancelled != null)
                m_ServerSpectator_movebackward.cancelled -= m_ServerSpectatorMovebackwardActionCancelled.Invoke;
            m_ServerSpectator_moveleft = null;
            if (m_ServerSpectatorMoveleftActionStarted != null)
                m_ServerSpectator_moveleft.started -= m_ServerSpectatorMoveleftActionStarted.Invoke;
            if (m_ServerSpectatorMoveleftActionPerformed != null)
                m_ServerSpectator_moveleft.performed -= m_ServerSpectatorMoveleftActionPerformed.Invoke;
            if (m_ServerSpectatorMoveleftActionCancelled != null)
                m_ServerSpectator_moveleft.cancelled -= m_ServerSpectatorMoveleftActionCancelled.Invoke;
            m_ServerSpectator_moveright = null;
            if (m_ServerSpectatorMoverightActionStarted != null)
                m_ServerSpectator_moveright.started -= m_ServerSpectatorMoverightActionStarted.Invoke;
            if (m_ServerSpectatorMoverightActionPerformed != null)
                m_ServerSpectator_moveright.performed -= m_ServerSpectatorMoverightActionPerformed.Invoke;
            if (m_ServerSpectatorMoverightActionCancelled != null)
                m_ServerSpectator_moveright.cancelled -= m_ServerSpectatorMoverightActionCancelled.Invoke;
            m_ServerSpectator_moveup = null;
            if (m_ServerSpectatorMoveupActionStarted != null)
                m_ServerSpectator_moveup.started -= m_ServerSpectatorMoveupActionStarted.Invoke;
            if (m_ServerSpectatorMoveupActionPerformed != null)
                m_ServerSpectator_moveup.performed -= m_ServerSpectatorMoveupActionPerformed.Invoke;
            if (m_ServerSpectatorMoveupActionCancelled != null)
                m_ServerSpectator_moveup.cancelled -= m_ServerSpectatorMoveupActionCancelled.Invoke;
            m_ServerSpectator_movedown = null;
            if (m_ServerSpectatorMovedownActionStarted != null)
                m_ServerSpectator_movedown.started -= m_ServerSpectatorMovedownActionStarted.Invoke;
            if (m_ServerSpectatorMovedownActionPerformed != null)
                m_ServerSpectator_movedown.performed -= m_ServerSpectatorMovedownActionPerformed.Invoke;
            if (m_ServerSpectatorMovedownActionCancelled != null)
                m_ServerSpectator_movedown.cancelled -= m_ServerSpectatorMovedownActionCancelled.Invoke;
            m_ServerSpectator_look = null;
            if (m_ServerSpectatorLookActionStarted != null)
                m_ServerSpectator_look.started -= m_ServerSpectatorLookActionStarted.Invoke;
            if (m_ServerSpectatorLookActionPerformed != null)
                m_ServerSpectator_look.performed -= m_ServerSpectatorLookActionPerformed.Invoke;
            if (m_ServerSpectatorLookActionCancelled != null)
                m_ServerSpectator_look.cancelled -= m_ServerSpectatorLookActionCancelled.Invoke;
            m_Initialized = false;
        }
        public void SetAsset(InputActionAsset newAsset)
        {
            if (newAsset == asset) return;
            var ServerSpectatorCallbacks = m_ServerSpectatorActionsCallbackInterface;
            if (m_Initialized) Uninitialize();
            asset = newAsset;
            ServerSpectator.SetCallbacks(ServerSpectatorCallbacks);
        }
        public override void MakePrivateCopyOfActions()
        {
            SetAsset(ScriptableObject.Instantiate(asset));
        }
        // ServerSpectator
        private InputActionMap m_ServerSpectator;
        private IServerSpectatorActions m_ServerSpectatorActionsCallbackInterface;
        private InputAction m_ServerSpectator_moveforward;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveforwardActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveforwardActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveforwardActionCancelled;
        private InputAction m_ServerSpectator_movebackward;
        [SerializeField] private ActionEvent m_ServerSpectatorMovebackwardActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMovebackwardActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMovebackwardActionCancelled;
        private InputAction m_ServerSpectator_moveleft;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveleftActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveleftActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveleftActionCancelled;
        private InputAction m_ServerSpectator_moveright;
        [SerializeField] private ActionEvent m_ServerSpectatorMoverightActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMoverightActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMoverightActionCancelled;
        private InputAction m_ServerSpectator_moveup;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveupActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveupActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMoveupActionCancelled;
        private InputAction m_ServerSpectator_movedown;
        [SerializeField] private ActionEvent m_ServerSpectatorMovedownActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorMovedownActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorMovedownActionCancelled;
        private InputAction m_ServerSpectator_look;
        [SerializeField] private ActionEvent m_ServerSpectatorLookActionStarted;
        [SerializeField] private ActionEvent m_ServerSpectatorLookActionPerformed;
        [SerializeField] private ActionEvent m_ServerSpectatorLookActionCancelled;
        public struct ServerSpectatorActions
        {
            private ServerSpectatorControls m_Wrapper;
            public ServerSpectatorActions(ServerSpectatorControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @moveforward { get { return m_Wrapper.m_ServerSpectator_moveforward; } }
            public ActionEvent moveforwardStarted { get { return m_Wrapper.m_ServerSpectatorMoveforwardActionStarted; } }
            public ActionEvent moveforwardPerformed { get { return m_Wrapper.m_ServerSpectatorMoveforwardActionPerformed; } }
            public ActionEvent moveforwardCancelled { get { return m_Wrapper.m_ServerSpectatorMoveforwardActionCancelled; } }
            public InputAction @movebackward { get { return m_Wrapper.m_ServerSpectator_movebackward; } }
            public ActionEvent movebackwardStarted { get { return m_Wrapper.m_ServerSpectatorMovebackwardActionStarted; } }
            public ActionEvent movebackwardPerformed { get { return m_Wrapper.m_ServerSpectatorMovebackwardActionPerformed; } }
            public ActionEvent movebackwardCancelled { get { return m_Wrapper.m_ServerSpectatorMovebackwardActionCancelled; } }
            public InputAction @moveleft { get { return m_Wrapper.m_ServerSpectator_moveleft; } }
            public ActionEvent moveleftStarted { get { return m_Wrapper.m_ServerSpectatorMoveleftActionStarted; } }
            public ActionEvent moveleftPerformed { get { return m_Wrapper.m_ServerSpectatorMoveleftActionPerformed; } }
            public ActionEvent moveleftCancelled { get { return m_Wrapper.m_ServerSpectatorMoveleftActionCancelled; } }
            public InputAction @moveright { get { return m_Wrapper.m_ServerSpectator_moveright; } }
            public ActionEvent moverightStarted { get { return m_Wrapper.m_ServerSpectatorMoverightActionStarted; } }
            public ActionEvent moverightPerformed { get { return m_Wrapper.m_ServerSpectatorMoverightActionPerformed; } }
            public ActionEvent moverightCancelled { get { return m_Wrapper.m_ServerSpectatorMoverightActionCancelled; } }
            public InputAction @moveup { get { return m_Wrapper.m_ServerSpectator_moveup; } }
            public ActionEvent moveupStarted { get { return m_Wrapper.m_ServerSpectatorMoveupActionStarted; } }
            public ActionEvent moveupPerformed { get { return m_Wrapper.m_ServerSpectatorMoveupActionPerformed; } }
            public ActionEvent moveupCancelled { get { return m_Wrapper.m_ServerSpectatorMoveupActionCancelled; } }
            public InputAction @movedown { get { return m_Wrapper.m_ServerSpectator_movedown; } }
            public ActionEvent movedownStarted { get { return m_Wrapper.m_ServerSpectatorMovedownActionStarted; } }
            public ActionEvent movedownPerformed { get { return m_Wrapper.m_ServerSpectatorMovedownActionPerformed; } }
            public ActionEvent movedownCancelled { get { return m_Wrapper.m_ServerSpectatorMovedownActionCancelled; } }
            public InputAction @look { get { return m_Wrapper.m_ServerSpectator_look; } }
            public ActionEvent lookStarted { get { return m_Wrapper.m_ServerSpectatorLookActionStarted; } }
            public ActionEvent lookPerformed { get { return m_Wrapper.m_ServerSpectatorLookActionPerformed; } }
            public ActionEvent lookCancelled { get { return m_Wrapper.m_ServerSpectatorLookActionCancelled; } }
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
                if (!m_Initialized) Initialize();
                return new ServerSpectatorActions(this);
            }
        }
        [Serializable]
        public class ActionEvent : UnityEvent<InputAction.CallbackContext>
        {
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
