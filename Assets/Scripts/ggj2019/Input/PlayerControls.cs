// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Player2d.inputactions'

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;


namespace pdxpartyparrot.ggj2019.Input
{
    [Serializable]
    public class PlayerControls : InputActionAssetReference
    {
        public PlayerControls()
        {
        }
        public PlayerControls(InputActionAsset asset)
            : base(asset)
        {
        }
        private bool m_Initialized;
        private void Initialize()
        {
            // Player
            m_Player = asset.GetActionMap("Player");
            m_Player_move = m_Player.GetAction("move");
            if (m_PlayerMoveActionStarted != null)
                m_Player_move.started += m_PlayerMoveActionStarted.Invoke;
            if (m_PlayerMoveActionPerformed != null)
                m_Player_move.performed += m_PlayerMoveActionPerformed.Invoke;
            if (m_PlayerMoveActionCancelled != null)
                m_Player_move.cancelled += m_PlayerMoveActionCancelled.Invoke;
            m_Player_Pause = m_Player.GetAction("Pause");
            if (m_PlayerPauseActionStarted != null)
                m_Player_Pause.started += m_PlayerPauseActionStarted.Invoke;
            if (m_PlayerPauseActionPerformed != null)
                m_Player_Pause.performed += m_PlayerPauseActionPerformed.Invoke;
            if (m_PlayerPauseActionCancelled != null)
                m_Player_Pause.cancelled += m_PlayerPauseActionCancelled.Invoke;
            m_Player_moveup = m_Player.GetAction("move up");
            if (m_PlayerMoveupActionStarted != null)
                m_Player_moveup.started += m_PlayerMoveupActionStarted.Invoke;
            if (m_PlayerMoveupActionPerformed != null)
                m_Player_moveup.performed += m_PlayerMoveupActionPerformed.Invoke;
            if (m_PlayerMoveupActionCancelled != null)
                m_Player_moveup.cancelled += m_PlayerMoveupActionCancelled.Invoke;
            m_Player_movedown = m_Player.GetAction("move down");
            if (m_PlayerMovedownActionStarted != null)
                m_Player_movedown.started += m_PlayerMovedownActionStarted.Invoke;
            if (m_PlayerMovedownActionPerformed != null)
                m_Player_movedown.performed += m_PlayerMovedownActionPerformed.Invoke;
            if (m_PlayerMovedownActionCancelled != null)
                m_Player_movedown.cancelled += m_PlayerMovedownActionCancelled.Invoke;
            m_Player_moveleft = m_Player.GetAction("move left");
            if (m_PlayerMoveleftActionStarted != null)
                m_Player_moveleft.started += m_PlayerMoveleftActionStarted.Invoke;
            if (m_PlayerMoveleftActionPerformed != null)
                m_Player_moveleft.performed += m_PlayerMoveleftActionPerformed.Invoke;
            if (m_PlayerMoveleftActionCancelled != null)
                m_Player_moveleft.cancelled += m_PlayerMoveleftActionCancelled.Invoke;
            m_Player_moveright = m_Player.GetAction("move right");
            if (m_PlayerMoverightActionStarted != null)
                m_Player_moveright.started += m_PlayerMoverightActionStarted.Invoke;
            if (m_PlayerMoverightActionPerformed != null)
                m_Player_moveright.performed += m_PlayerMoverightActionPerformed.Invoke;
            if (m_PlayerMoverightActionCancelled != null)
                m_Player_moveright.cancelled += m_PlayerMoverightActionCancelled.Invoke;
            m_Player_gather = m_Player.GetAction("gather");
            if (m_PlayerGatherActionStarted != null)
                m_Player_gather.started += m_PlayerGatherActionStarted.Invoke;
            if (m_PlayerGatherActionPerformed != null)
                m_Player_gather.performed += m_PlayerGatherActionPerformed.Invoke;
            if (m_PlayerGatherActionCancelled != null)
                m_Player_gather.cancelled += m_PlayerGatherActionCancelled.Invoke;
            m_Player_context = m_Player.GetAction("context");
            if (m_PlayerContextActionStarted != null)
                m_Player_context.started += m_PlayerContextActionStarted.Invoke;
            if (m_PlayerContextActionPerformed != null)
                m_Player_context.performed += m_PlayerContextActionPerformed.Invoke;
            if (m_PlayerContextActionCancelled != null)
                m_Player_context.cancelled += m_PlayerContextActionCancelled.Invoke;
            m_Player_movedpad = m_Player.GetAction("movedpad");
            if (m_PlayerMovedpadActionStarted != null)
                m_Player_movedpad.started += m_PlayerMovedpadActionStarted.Invoke;
            if (m_PlayerMovedpadActionPerformed != null)
                m_Player_movedpad.performed += m_PlayerMovedpadActionPerformed.Invoke;
            if (m_PlayerMovedpadActionCancelled != null)
                m_Player_movedpad.cancelled += m_PlayerMovedpadActionCancelled.Invoke;
            m_Initialized = true;
        }
        private void Uninitialize()
        {
            if (m_PlayerActionsCallbackInterface != null)
            {
                Player.SetCallbacks(null);
            }
            m_Player = null;
            m_Player_move = null;
            if (m_PlayerMoveActionStarted != null)
                m_Player_move.started -= m_PlayerMoveActionStarted.Invoke;
            if (m_PlayerMoveActionPerformed != null)
                m_Player_move.performed -= m_PlayerMoveActionPerformed.Invoke;
            if (m_PlayerMoveActionCancelled != null)
                m_Player_move.cancelled -= m_PlayerMoveActionCancelled.Invoke;
            m_Player_Pause = null;
            if (m_PlayerPauseActionStarted != null)
                m_Player_Pause.started -= m_PlayerPauseActionStarted.Invoke;
            if (m_PlayerPauseActionPerformed != null)
                m_Player_Pause.performed -= m_PlayerPauseActionPerformed.Invoke;
            if (m_PlayerPauseActionCancelled != null)
                m_Player_Pause.cancelled -= m_PlayerPauseActionCancelled.Invoke;
            m_Player_moveup = null;
            if (m_PlayerMoveupActionStarted != null)
                m_Player_moveup.started -= m_PlayerMoveupActionStarted.Invoke;
            if (m_PlayerMoveupActionPerformed != null)
                m_Player_moveup.performed -= m_PlayerMoveupActionPerformed.Invoke;
            if (m_PlayerMoveupActionCancelled != null)
                m_Player_moveup.cancelled -= m_PlayerMoveupActionCancelled.Invoke;
            m_Player_movedown = null;
            if (m_PlayerMovedownActionStarted != null)
                m_Player_movedown.started -= m_PlayerMovedownActionStarted.Invoke;
            if (m_PlayerMovedownActionPerformed != null)
                m_Player_movedown.performed -= m_PlayerMovedownActionPerformed.Invoke;
            if (m_PlayerMovedownActionCancelled != null)
                m_Player_movedown.cancelled -= m_PlayerMovedownActionCancelled.Invoke;
            m_Player_moveleft = null;
            if (m_PlayerMoveleftActionStarted != null)
                m_Player_moveleft.started -= m_PlayerMoveleftActionStarted.Invoke;
            if (m_PlayerMoveleftActionPerformed != null)
                m_Player_moveleft.performed -= m_PlayerMoveleftActionPerformed.Invoke;
            if (m_PlayerMoveleftActionCancelled != null)
                m_Player_moveleft.cancelled -= m_PlayerMoveleftActionCancelled.Invoke;
            m_Player_moveright = null;
            if (m_PlayerMoverightActionStarted != null)
                m_Player_moveright.started -= m_PlayerMoverightActionStarted.Invoke;
            if (m_PlayerMoverightActionPerformed != null)
                m_Player_moveright.performed -= m_PlayerMoverightActionPerformed.Invoke;
            if (m_PlayerMoverightActionCancelled != null)
                m_Player_moveright.cancelled -= m_PlayerMoverightActionCancelled.Invoke;
            m_Player_gather = null;
            if (m_PlayerGatherActionStarted != null)
                m_Player_gather.started -= m_PlayerGatherActionStarted.Invoke;
            if (m_PlayerGatherActionPerformed != null)
                m_Player_gather.performed -= m_PlayerGatherActionPerformed.Invoke;
            if (m_PlayerGatherActionCancelled != null)
                m_Player_gather.cancelled -= m_PlayerGatherActionCancelled.Invoke;
            m_Player_context = null;
            if (m_PlayerContextActionStarted != null)
                m_Player_context.started -= m_PlayerContextActionStarted.Invoke;
            if (m_PlayerContextActionPerformed != null)
                m_Player_context.performed -= m_PlayerContextActionPerformed.Invoke;
            if (m_PlayerContextActionCancelled != null)
                m_Player_context.cancelled -= m_PlayerContextActionCancelled.Invoke;
            m_Player_movedpad = null;
            if (m_PlayerMovedpadActionStarted != null)
                m_Player_movedpad.started -= m_PlayerMovedpadActionStarted.Invoke;
            if (m_PlayerMovedpadActionPerformed != null)
                m_Player_movedpad.performed -= m_PlayerMovedpadActionPerformed.Invoke;
            if (m_PlayerMovedpadActionCancelled != null)
                m_Player_movedpad.cancelled -= m_PlayerMovedpadActionCancelled.Invoke;
            m_Initialized = false;
        }
        public void SetAsset(InputActionAsset newAsset)
        {
            if (newAsset == asset) return;
            var PlayerCallbacks = m_PlayerActionsCallbackInterface;
            if (m_Initialized) Uninitialize();
            asset = newAsset;
            Player.SetCallbacks(PlayerCallbacks);
        }
        public override void MakePrivateCopyOfActions()
        {
            SetAsset(ScriptableObject.Instantiate(asset));
        }
        // Player
        private InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private InputAction m_Player_move;
        [SerializeField] private ActionEvent m_PlayerMoveActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoveActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoveActionCancelled;
        private InputAction m_Player_Pause;
        [SerializeField] private ActionEvent m_PlayerPauseActionStarted;
        [SerializeField] private ActionEvent m_PlayerPauseActionPerformed;
        [SerializeField] private ActionEvent m_PlayerPauseActionCancelled;
        private InputAction m_Player_moveup;
        [SerializeField] private ActionEvent m_PlayerMoveupActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoveupActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoveupActionCancelled;
        private InputAction m_Player_movedown;
        [SerializeField] private ActionEvent m_PlayerMovedownActionStarted;
        [SerializeField] private ActionEvent m_PlayerMovedownActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMovedownActionCancelled;
        private InputAction m_Player_moveleft;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionCancelled;
        private InputAction m_Player_moveright;
        [SerializeField] private ActionEvent m_PlayerMoverightActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoverightActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoverightActionCancelled;
        private InputAction m_Player_gather;
        [SerializeField] private ActionEvent m_PlayerGatherActionStarted;
        [SerializeField] private ActionEvent m_PlayerGatherActionPerformed;
        [SerializeField] private ActionEvent m_PlayerGatherActionCancelled;
        private InputAction m_Player_context;
        [SerializeField] private ActionEvent m_PlayerContextActionStarted;
        [SerializeField] private ActionEvent m_PlayerContextActionPerformed;
        [SerializeField] private ActionEvent m_PlayerContextActionCancelled;
        private InputAction m_Player_movedpad;
        [SerializeField] private ActionEvent m_PlayerMovedpadActionStarted;
        [SerializeField] private ActionEvent m_PlayerMovedpadActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMovedpadActionCancelled;
        public struct PlayerActions
        {
            private PlayerControls m_Wrapper;
            public PlayerActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @move { get { return m_Wrapper.m_Player_move; } }
            public ActionEvent moveStarted { get { return m_Wrapper.m_PlayerMoveActionStarted; } }
            public ActionEvent movePerformed { get { return m_Wrapper.m_PlayerMoveActionPerformed; } }
            public ActionEvent moveCancelled { get { return m_Wrapper.m_PlayerMoveActionCancelled; } }
            public InputAction @Pause { get { return m_Wrapper.m_Player_Pause; } }
            public ActionEvent PauseStarted { get { return m_Wrapper.m_PlayerPauseActionStarted; } }
            public ActionEvent PausePerformed { get { return m_Wrapper.m_PlayerPauseActionPerformed; } }
            public ActionEvent PauseCancelled { get { return m_Wrapper.m_PlayerPauseActionCancelled; } }
            public InputAction @moveup { get { return m_Wrapper.m_Player_moveup; } }
            public ActionEvent moveupStarted { get { return m_Wrapper.m_PlayerMoveupActionStarted; } }
            public ActionEvent moveupPerformed { get { return m_Wrapper.m_PlayerMoveupActionPerformed; } }
            public ActionEvent moveupCancelled { get { return m_Wrapper.m_PlayerMoveupActionCancelled; } }
            public InputAction @movedown { get { return m_Wrapper.m_Player_movedown; } }
            public ActionEvent movedownStarted { get { return m_Wrapper.m_PlayerMovedownActionStarted; } }
            public ActionEvent movedownPerformed { get { return m_Wrapper.m_PlayerMovedownActionPerformed; } }
            public ActionEvent movedownCancelled { get { return m_Wrapper.m_PlayerMovedownActionCancelled; } }
            public InputAction @moveleft { get { return m_Wrapper.m_Player_moveleft; } }
            public ActionEvent moveleftStarted { get { return m_Wrapper.m_PlayerMoveleftActionStarted; } }
            public ActionEvent moveleftPerformed { get { return m_Wrapper.m_PlayerMoveleftActionPerformed; } }
            public ActionEvent moveleftCancelled { get { return m_Wrapper.m_PlayerMoveleftActionCancelled; } }
            public InputAction @moveright { get { return m_Wrapper.m_Player_moveright; } }
            public ActionEvent moverightStarted { get { return m_Wrapper.m_PlayerMoverightActionStarted; } }
            public ActionEvent moverightPerformed { get { return m_Wrapper.m_PlayerMoverightActionPerformed; } }
            public ActionEvent moverightCancelled { get { return m_Wrapper.m_PlayerMoverightActionCancelled; } }
            public InputAction @gather { get { return m_Wrapper.m_Player_gather; } }
            public ActionEvent gatherStarted { get { return m_Wrapper.m_PlayerGatherActionStarted; } }
            public ActionEvent gatherPerformed { get { return m_Wrapper.m_PlayerGatherActionPerformed; } }
            public ActionEvent gatherCancelled { get { return m_Wrapper.m_PlayerGatherActionCancelled; } }
            public InputAction @context { get { return m_Wrapper.m_Player_context; } }
            public ActionEvent contextStarted { get { return m_Wrapper.m_PlayerContextActionStarted; } }
            public ActionEvent contextPerformed { get { return m_Wrapper.m_PlayerContextActionPerformed; } }
            public ActionEvent contextCancelled { get { return m_Wrapper.m_PlayerContextActionCancelled; } }
            public InputAction @movedpad { get { return m_Wrapper.m_Player_movedpad; } }
            public ActionEvent movedpadStarted { get { return m_Wrapper.m_PlayerMovedpadActionStarted; } }
            public ActionEvent movedpadPerformed { get { return m_Wrapper.m_PlayerMovedpadActionPerformed; } }
            public ActionEvent movedpadCancelled { get { return m_Wrapper.m_PlayerMovedpadActionCancelled; } }
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
                if (!m_Initialized) Initialize();
                return new PlayerActions(this);
            }
        }
        [Serializable]
        public class ActionEvent : UnityEvent<InputAction.CallbackContext>
        {
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
