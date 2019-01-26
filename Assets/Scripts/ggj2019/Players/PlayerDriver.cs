﻿using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Actors.ControllerComponents;
using pdxpartyparrot.ggj2019Input;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerDriver : Game.Players.PlayerDriver, IPlayerActions
    {
        private const string InvertLookYKey = "playerdriver.invertlooky";

        [SerializeField]
        private PlayerControls _controls;

        private bool InvertLookY
        {
            get => PartyParrotManager.Instance.GetBool(InvertLookYKey);
            set => PartyParrotManager.Instance.SetBool(InvertLookYKey, value);
        }

        private PlayerController GamePlayerController => (PlayerController)PlayerController;

        private Player GamePlayer => GamePlayerController.GamePlayer;

        private GamepadListener _gamepadListener;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerController is PlayerController);
            Assert.IsNull(GetComponent<GamepadListener>());
        }

        protected override void Update()
        {
            base.Update();

            if(!Player.IsLocalActor) {
                return;
            }

            float dt = Time.deltaTime;

            GamePlayer.FollowTarget.LastLookAxes = Vector3.Lerp(GamePlayer.FollowTarget.LastLookAxes, LastControllerLook, dt * PlayerManager.Instance.PlayerData.LookLerpSpeed);
        }

        protected override void OnDestroy()
        {
            _controls.Player.Disable();
            _controls.Player.SetCallbacks(null);

            Destroy(_gamepadListener);
            _gamepadListener = null;

            DestroyDebugMenu();

            base.OnDestroy();
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();

            if(!Player.IsLocalActor) {
                return;
            }

            _gamepadListener = gameObject.AddComponent<GamepadListener>();

            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();

            InitDebugMenu();
        }

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // no input unless we have focus
            if(!Application.isFocused) {
                return false;
            }

            return _gamepadListener.IsOurGamepad(ctx) ||
                // ignore keyboard/mouse while the debug menu is open
                // TODO: this probably doesn't handle multiple keyboards/mice
                (!DebugMenuManager.Instance.Enabled && (Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device));
        }

#region IPlayerActions
        public void OnPause(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            if(context.performed) {
                PartyParrotManager.Instance.TogglePause();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(context.cancelled) {
                LastControllerMove = Vector3.zero;
                Controller.LastMoveAxes = Vector3.zero;
            } else {
                Vector2 axes = context.ReadValue<Vector2>();
                LastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
            }
        }

        public void OnMoveforward(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.started ? 1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                Controller.LastMoveAxes = LastControllerMove;
            }
        }

        public void OnMovebackward(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.started ? -1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                Controller.LastMoveAxes = LastControllerMove;
            }
        }

        public void OnMoveleft(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.started ? -1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                Controller.LastMoveAxes = LastControllerMove;
            }
        }

        public void OnMoveright(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.started ? 1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                Controller.LastMoveAxes = LastControllerMove;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            bool isMouse = context.control.device is Mouse;
            if(!IsOurDevice(context) || !CanDrive || (isMouse && !EnableMouseLook)) {
                return;
            }

            if(context.cancelled) {
                LastControllerLook = Vector3.zero;
                GamePlayer.FollowTarget.LastLookAxes = Vector3.zero;
            } else {
                Vector2 axes = context.ReadValue<Vector2>();
                axes.y *= InvertLookY ? -1 : 1;

                if(isMouse) {
                    axes *= MouseSensitivity;
                }

                LastControllerLook = new Vector3(axes.x, axes.y, 0.0f);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(context.started) {
                GamePlayerController.CharacterController.ActionStarted(JumpControllerComponent.JumpAction.Default);
            } else if(context.performed) {
                GamePlayerController.CharacterController.ActionPerformed(JumpControllerComponent.JumpAction.Default);
            }
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"ggj2019.Player {Player.Name} Driver");
            _debugMenuNode.RenderContentsAction = () => {
                InvertLookY = GUILayout.Toggle(InvertLookY, "Invert Look Y");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}