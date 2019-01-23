// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Player3d.inputactions'

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;


namespace pdxpartyparrot.ggj2019Input
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
            m_Player_look = m_Player.GetAction("look");
            if (m_PlayerLookActionStarted != null)
                m_Player_look.started += m_PlayerLookActionStarted.Invoke;
            if (m_PlayerLookActionPerformed != null)
                m_Player_look.performed += m_PlayerLookActionPerformed.Invoke;
            if (m_PlayerLookActionCancelled != null)
                m_Player_look.cancelled += m_PlayerLookActionCancelled.Invoke;
            m_Player_jump = m_Player.GetAction("jump");
            if (m_PlayerJumpActionStarted != null)
                m_Player_jump.started += m_PlayerJumpActionStarted.Invoke;
            if (m_PlayerJumpActionPerformed != null)
                m_Player_jump.performed += m_PlayerJumpActionPerformed.Invoke;
            if (m_PlayerJumpActionCancelled != null)
                m_Player_jump.cancelled += m_PlayerJumpActionCancelled.Invoke;
            m_Player_Pause = m_Player.GetAction("Pause");
            if (m_PlayerPauseActionStarted != null)
                m_Player_Pause.started += m_PlayerPauseActionStarted.Invoke;
            if (m_PlayerPauseActionPerformed != null)
                m_Player_Pause.performed += m_PlayerPauseActionPerformed.Invoke;
            if (m_PlayerPauseActionCancelled != null)
                m_Player_Pause.cancelled += m_PlayerPauseActionCancelled.Invoke;
            m_Player_moveforward = m_Player.GetAction("move forward");
            if (m_PlayerMoveforwardActionStarted != null)
                m_Player_moveforward.started += m_PlayerMoveforwardActionStarted.Invoke;
            if (m_PlayerMoveforwardActionPerformed != null)
                m_Player_moveforward.performed += m_PlayerMoveforwardActionPerformed.Invoke;
            if (m_PlayerMoveforwardActionCancelled != null)
                m_Player_moveforward.cancelled += m_PlayerMoveforwardActionCancelled.Invoke;
            m_Player_movebackward = m_Player.GetAction("move backward");
            if (m_PlayerMovebackwardActionStarted != null)
                m_Player_movebackward.started += m_PlayerMovebackwardActionStarted.Invoke;
            if (m_PlayerMovebackwardActionPerformed != null)
                m_Player_movebackward.performed += m_PlayerMovebackwardActionPerformed.Invoke;
            if (m_PlayerMovebackwardActionCancelled != null)
                m_Player_movebackward.cancelled += m_PlayerMovebackwardActionCancelled.Invoke;
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
            m_Player_look = null;
            if (m_PlayerLookActionStarted != null)
                m_Player_look.started -= m_PlayerLookActionStarted.Invoke;
            if (m_PlayerLookActionPerformed != null)
                m_Player_look.performed -= m_PlayerLookActionPerformed.Invoke;
            if (m_PlayerLookActionCancelled != null)
                m_Player_look.cancelled -= m_PlayerLookActionCancelled.Invoke;
            m_Player_jump = null;
            if (m_PlayerJumpActionStarted != null)
                m_Player_jump.started -= m_PlayerJumpActionStarted.Invoke;
            if (m_PlayerJumpActionPerformed != null)
                m_Player_jump.performed -= m_PlayerJumpActionPerformed.Invoke;
            if (m_PlayerJumpActionCancelled != null)
                m_Player_jump.cancelled -= m_PlayerJumpActionCancelled.Invoke;
            m_Player_Pause = null;
            if (m_PlayerPauseActionStarted != null)
                m_Player_Pause.started -= m_PlayerPauseActionStarted.Invoke;
            if (m_PlayerPauseActionPerformed != null)
                m_Player_Pause.performed -= m_PlayerPauseActionPerformed.Invoke;
            if (m_PlayerPauseActionCancelled != null)
                m_Player_Pause.cancelled -= m_PlayerPauseActionCancelled.Invoke;
            m_Player_moveforward = null;
            if (m_PlayerMoveforwardActionStarted != null)
                m_Player_moveforward.started -= m_PlayerMoveforwardActionStarted.Invoke;
            if (m_PlayerMoveforwardActionPerformed != null)
                m_Player_moveforward.performed -= m_PlayerMoveforwardActionPerformed.Invoke;
            if (m_PlayerMoveforwardActionCancelled != null)
                m_Player_moveforward.cancelled -= m_PlayerMoveforwardActionCancelled.Invoke;
            m_Player_movebackward = null;
            if (m_PlayerMovebackwardActionStarted != null)
                m_Player_movebackward.started -= m_PlayerMovebackwardActionStarted.Invoke;
            if (m_PlayerMovebackwardActionPerformed != null)
                m_Player_movebackward.performed -= m_PlayerMovebackwardActionPerformed.Invoke;
            if (m_PlayerMovebackwardActionCancelled != null)
                m_Player_movebackward.cancelled -= m_PlayerMovebackwardActionCancelled.Invoke;
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
        private InputAction m_Player_look;
        [SerializeField] private ActionEvent m_PlayerLookActionStarted;
        [SerializeField] private ActionEvent m_PlayerLookActionPerformed;
        [SerializeField] private ActionEvent m_PlayerLookActionCancelled;
        private InputAction m_Player_jump;
        [SerializeField] private ActionEvent m_PlayerJumpActionStarted;
        [SerializeField] private ActionEvent m_PlayerJumpActionPerformed;
        [SerializeField] private ActionEvent m_PlayerJumpActionCancelled;
        private InputAction m_Player_Pause;
        [SerializeField] private ActionEvent m_PlayerPauseActionStarted;
        [SerializeField] private ActionEvent m_PlayerPauseActionPerformed;
        [SerializeField] private ActionEvent m_PlayerPauseActionCancelled;
        private InputAction m_Player_moveforward;
        [SerializeField] private ActionEvent m_PlayerMoveforwardActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoveforwardActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoveforwardActionCancelled;
        private InputAction m_Player_movebackward;
        [SerializeField] private ActionEvent m_PlayerMovebackwardActionStarted;
        [SerializeField] private ActionEvent m_PlayerMovebackwardActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMovebackwardActionCancelled;
        private InputAction m_Player_moveleft;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoveleftActionCancelled;
        private InputAction m_Player_moveright;
        [SerializeField] private ActionEvent m_PlayerMoverightActionStarted;
        [SerializeField] private ActionEvent m_PlayerMoverightActionPerformed;
        [SerializeField] private ActionEvent m_PlayerMoverightActionCancelled;
        public struct PlayerActions
        {
            private PlayerControls m_Wrapper;
            public PlayerActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @move { get { return m_Wrapper.m_Player_move; } }
            public ActionEvent moveStarted { get { return m_Wrapper.m_PlayerMoveActionStarted; } }
            public ActionEvent movePerformed { get { return m_Wrapper.m_PlayerMoveActionPerformed; } }
            public ActionEvent moveCancelled { get { return m_Wrapper.m_PlayerMoveActionCancelled; } }
            public InputAction @look { get { return m_Wrapper.m_Player_look; } }
            public ActionEvent lookStarted { get { return m_Wrapper.m_PlayerLookActionStarted; } }
            public ActionEvent lookPerformed { get { return m_Wrapper.m_PlayerLookActionPerformed; } }
            public ActionEvent lookCancelled { get { return m_Wrapper.m_PlayerLookActionCancelled; } }
            public InputAction @jump { get { return m_Wrapper.m_Player_jump; } }
            public ActionEvent jumpStarted { get { return m_Wrapper.m_PlayerJumpActionStarted; } }
            public ActionEvent jumpPerformed { get { return m_Wrapper.m_PlayerJumpActionPerformed; } }
            public ActionEvent jumpCancelled { get { return m_Wrapper.m_PlayerJumpActionCancelled; } }
            public InputAction @Pause { get { return m_Wrapper.m_Player_Pause; } }
            public ActionEvent PauseStarted { get { return m_Wrapper.m_PlayerPauseActionStarted; } }
            public ActionEvent PausePerformed { get { return m_Wrapper.m_PlayerPauseActionPerformed; } }
            public ActionEvent PauseCancelled { get { return m_Wrapper.m_PlayerPauseActionCancelled; } }
            public InputAction @moveforward { get { return m_Wrapper.m_Player_moveforward; } }
            public ActionEvent moveforwardStarted { get { return m_Wrapper.m_PlayerMoveforwardActionStarted; } }
            public ActionEvent moveforwardPerformed { get { return m_Wrapper.m_PlayerMoveforwardActionPerformed; } }
            public ActionEvent moveforwardCancelled { get { return m_Wrapper.m_PlayerMoveforwardActionCancelled; } }
            public InputAction @movebackward { get { return m_Wrapper.m_Player_movebackward; } }
            public ActionEvent movebackwardStarted { get { return m_Wrapper.m_PlayerMovebackwardActionStarted; } }
            public ActionEvent movebackwardPerformed { get { return m_Wrapper.m_PlayerMovebackwardActionPerformed; } }
            public ActionEvent movebackwardCancelled { get { return m_Wrapper.m_PlayerMovebackwardActionCancelled; } }
            public InputAction @moveleft { get { return m_Wrapper.m_Player_moveleft; } }
            public ActionEvent moveleftStarted { get { return m_Wrapper.m_PlayerMoveleftActionStarted; } }
            public ActionEvent moveleftPerformed { get { return m_Wrapper.m_PlayerMoveleftActionPerformed; } }
            public ActionEvent moveleftCancelled { get { return m_Wrapper.m_PlayerMoveleftActionCancelled; } }
            public InputAction @moveright { get { return m_Wrapper.m_Player_moveright; } }
            public ActionEvent moverightStarted { get { return m_Wrapper.m_PlayerMoverightActionStarted; } }
            public ActionEvent moverightPerformed { get { return m_Wrapper.m_PlayerMoverightActionPerformed; } }
            public ActionEvent moverightCancelled { get { return m_Wrapper.m_PlayerMoverightActionCancelled; } }
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
                    look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    look.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    jump.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    Pause.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                    moveforward.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveforward;
                    moveforward.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveforward;
                    moveforward.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveforward;
                    movebackward.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovebackward;
                    movebackward.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovebackward;
                    movebackward.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovebackward;
                    moveleft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveleft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveleft.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveleft;
                    moveright.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                    moveright.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                    moveright.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveright;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    move.started += instance.OnMove;
                    move.performed += instance.OnMove;
                    move.cancelled += instance.OnMove;
                    look.started += instance.OnLook;
                    look.performed += instance.OnLook;
                    look.cancelled += instance.OnLook;
                    jump.started += instance.OnJump;
                    jump.performed += instance.OnJump;
                    jump.cancelled += instance.OnJump;
                    Pause.started += instance.OnPause;
                    Pause.performed += instance.OnPause;
                    Pause.cancelled += instance.OnPause;
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
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnMoveforward(InputAction.CallbackContext context);
        void OnMovebackward(InputAction.CallbackContext context);
        void OnMoveleft(InputAction.CallbackContext context);
        void OnMoveright(InputAction.CallbackContext context);
    }
}
