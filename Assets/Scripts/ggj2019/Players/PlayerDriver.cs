using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.Actors;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerDriver : Game.Players.PlayerDriver
    {
        private const string InvertLookYKey = "playerdriver.invertlooky";

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
            Destroy(_gamepadListener);
            _gamepadListener = null;

            DestroyDebugMenu();

// TODO: where did enable / disable controls go?
// we should mirror that also for the server spectator rather than using just unity enable / disable callbacks

Debug.LogWarning("TODO: player input");
/*
            if(InputManager.HasInstance) {
                InputManager.Instance.Controls.game.pause.performed -= OnPause;

                InputManager.Instance.Controls.game.move.started -= OnMove;
                InputManager.Instance.Controls.game.move.performed -= OnMove;
                InputManager.Instance.Controls.game.move.cancelled -= OnMoveStop;

                InputManager.Instance.Controls.game.moveforward.started -= OnMoveForward;
                InputManager.Instance.Controls.game.moveforward.performed -= OnMoveForwardStop;
                InputManager.Instance.Controls.game.movebackward.started -= OnMoveBackward;
                InputManager.Instance.Controls.game.movebackward.performed -= OnMoveBackwardStop;
                InputManager.Instance.Controls.game.moveleft.started -= OnMoveLeft;
                InputManager.Instance.Controls.game.moveleft.performed -= OnMoveLeftStop;
                InputManager.Instance.Controls.game.moveright.started -= OnMoveRight;
                InputManager.Instance.Controls.game.moveright.performed -= OnMoveRightStop;

                InputManager.Instance.Controls.game.look.started -= OnLook;
                InputManager.Instance.Controls.game.look.performed -= OnLook;
                InputManager.Instance.Controls.game.look.cancelled -= OnLookStop;

                InputManager.Instance.Controls.game.jump.started -= OnJumpStart;
                InputManager.Instance.Controls.game.jump.performed -= OnJump;
            }
*/

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

Debug.LogWarning("TODO: player input");
/*
            InputManager.Instance.Controls.game.pause.performed += OnPause;

            InputManager.Instance.Controls.game.move.started += OnMove;
            InputManager.Instance.Controls.game.move.performed += OnMove;
            InputManager.Instance.Controls.game.move.cancelled += OnMoveStop;

            InputManager.Instance.Controls.game.moveforward.started += OnMoveForward;
            InputManager.Instance.Controls.game.moveforward.performed += OnMoveForwardStop;
            InputManager.Instance.Controls.game.movebackward.started += OnMoveBackward;
            InputManager.Instance.Controls.game.movebackward.performed += OnMoveBackwardStop;
            InputManager.Instance.Controls.game.moveleft.started += OnMoveLeft;
            InputManager.Instance.Controls.game.moveleft.performed += OnMoveLeftStop;
            InputManager.Instance.Controls.game.moveright.started += OnMoveRight;
            InputManager.Instance.Controls.game.moveright.performed += OnMoveRightStop;

            InputManager.Instance.Controls.game.look.started += OnLook;
            InputManager.Instance.Controls.game.look.performed += OnLook;
            InputManager.Instance.Controls.game.look.cancelled += OnLookStop;

            InputManager.Instance.Controls.game.jump.started += OnJumpStart;
            InputManager.Instance.Controls.game.jump.performed += OnJump;
*/

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
// TODO
/*
#region Event Handlers
        private void OnPause(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx)) {
                return;
            }

            PartyParrotManager.Instance.TogglePause();
        }

#region Gamepad Move
        private void OnMove(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            _lastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnMoveStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = Vector3.zero;
            Controller.LastMoveAxes = _lastControllerMove;
        }
#endregion

#region Keyboard Move
        private void OnMoveForward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 1.0f, 0.0f);
        }

        private void OnMoveForwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, 0.0f);
            Controller.LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveBackward(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, -1.0f, 0.0f);
        }

        private void OnMoveBackwardStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, 0.0f, 0.0f);
            Controller.LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveLeft(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(-1.0f, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveLeftStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, 0.0f);
            Controller.LastMoveAxes = _lastControllerMove;
        }

        private void OnMoveRight(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(1.0f, _lastControllerMove.y, 0.0f);
        }

        private void OnMoveRightStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerMove = new Vector3(0.0f, _lastControllerMove.y, 0.0f);
            Controller.LastMoveAxes = _lastControllerMove;
        }
#endregion

#region Look
        private void OnLook(InputAction.CallbackContext ctx)
        {
            bool isMouse = ctx.control.device is Mouse;
            if(!IsOurDevice(ctx) || !CanDrive || (isMouse && !_enableMouseLook)) {
                return;
            }

            Vector2 axes = ctx.ReadValue<Vector2>();
            axes.y *= InvertLookY ? -1 : 1;

            if(isMouse) {
                axes *= _mouseSensitivity;
            }

            _lastControllerLook = new Vector3(axes.x, axes.y, 0.0f);
        }

        private void OnLookStop(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            _lastControllerLook = Vector3.zero;
            Player.FollowTarget.LastLookAxes = _lastControllerLook;
        }
#endregion

#region Jump
        private void OnJumpStart(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionStarted(JumpControllerComponent.JumpAction.Default);
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if(!IsOurDevice(ctx) || !CanDrive) {
                return;
            }

            Player.PlayerController.ActionPerformed(JumpControllerComponent.JumpAction.Default);
        }
#endregion

#endregion
*/

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
