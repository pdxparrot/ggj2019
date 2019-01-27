using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.ggj2019.Input;
using pdxpartyparrot.ggj2019.Players.ControllerComponents;
using Spine.Unity;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerDriver : Game.Players.PlayerDriver, IPlayerActions, IPauseActionHandler
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

        protected override bool CanDrive => base.CanDrive && !GamePlayer.IsDead;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerController is PlayerController);
            Assert.IsNull(GetComponent<GamepadListener>());
        }

        protected override void OnDestroy()
        {
            _controls.Player.SetCallbacks(null);
            _controls.Player.Disable();

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

            _controls.MakePrivateCopyOfActions();
            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();

            _gamepadListener = gameObject.AddComponent<GamepadListener>();

            InitDebugMenu();

            _animation.Skeleton.ScaleX = transform.position.x > 0 ? 1.0f : -1.0f;
            SetHoverAnimation();
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
                (!DebugMenuManager.Instance.Enabled && PlayerManager.Instance.PlayerCount == 1 && (Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device));
        }

        [SerializeField]
        private SkeletonAnimation _animation;

        // start true to force the animation the first time
        private bool _isFlying = true;

        private void SetHoverAnimation()
        {
            if(!_isFlying) {
                return;
            }

            SetAnimation("bee_hover", true);
            _isFlying = false;
        }

        private void SetFlightAnimation()
        {
            if(_isFlying) {
                return;
            }

            SetAnimation("bee-flight", true);
            _isFlying = true;
        }

        private void SetAnimation(string animationName, bool loop)
        {
            _animation.AnimationState.SetAnimation(0, animationName, loop);
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
                SetHoverAnimation();

                LastControllerMove = Vector3.zero;
                Controller.LastMoveAxes = Vector3.zero;
            } else {
                SetFlightAnimation();

                Vector2 axes = context.ReadValue<Vector2>();
                LastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
                if(LastControllerMove.sqrMagnitude < float.Epsilon) {
                    SetHoverAnimation();
                } else {
                    _animation.Skeleton.ScaleX = LastControllerMove.x < 0 ? -1.0f : 1.0f;
                }
            }
        }

#region TODO: keyboard animation shit
        public void OnMoveup(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.started ? 1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                Controller.LastMoveAxes = LastControllerMove;
            }
        }

        public void OnMovedown(InputAction.CallbackContext context)
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
#endregion

        public void OnGather(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(context.started) {
                GamePlayerController.CharacterController.ActionStarted(GatherControllerComponent.GatherAction.Default);
            }
        }

        public void OnContext(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(context.started) {
                GamePlayerController.CharacterController.ActionStarted(ContextControllerComponent.ContextAction.Default);
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
