using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.ggj2019.Input;
using pdxpartyparrot.ggj2019.Players.BehaviorComponents;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerDriver : Game.Players.PlayerDriver, IPlayerActions, IPauseActionHandler
    {
        [SerializeField]
        private PlayerControls _controls;

        private PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        private Player GamePlayer => GamePlayerBehavior.GamePlayer;

        public GamepadListener GamepadListener { get; private set; }

        protected override bool CanDrive => base.CanDrive && !GamePlayer.IsDead && !GameManager.Instance.IsGameOver;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
            Assert.IsNull(GetComponent<GamepadListener>());
        }

        protected override void OnDestroy()
        {
            _controls.Player.SetCallbacks(null);
            _controls.Player.Disable();

            Destroy(GamepadListener);
            GamepadListener = null;

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

            GamepadListener = gameObject.AddComponent<GamepadListener>();
        }

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // no input unless we have focus
            if(!Application.isFocused) {
                return false;
            }

            return GamepadListener.IsOurGamepad(ctx) ||
                // ignore keyboard/mouse while the debug menu is open
                // TODO: this probably doesn't handle multiple keyboards/mice
                (!DebugMenuManager.Instance.Enabled && ActorManager.Instance.ActorCount<Player>() == 1 && (Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device));
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

            // relying in input system binding set to continuous for this
            Vector2 axes = context.ReadValue<Vector2>();
            LastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
        }

        public void OnMovedpad(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            // relying in input system binding set to continuous for this
            Vector2 axes = context.ReadValue<Vector2>();
            LastControllerMove = new Vector3(axes.x, axes.y, 0.0f);
        }

#region Keyboard controls
        public void OnMoveup(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.performed ? 1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                GamePlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMovedown(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.performed ? -1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                GamePlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMoveleft(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.performed ? -1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                GamePlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMoveright(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.performed ? 1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                GamePlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }
#endregion

        public void OnGather(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            // action on release
            if(context.cancelled) {
                GamePlayerBehavior.ActionPerformed(GatherBehaviorComponent.GatherAction.Default);
            }
        }

        public void OnContext(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            // action on release
            if(context.cancelled) {
                GamePlayerBehavior.ActionPerformed(ContextBehaviorComponent.ContextAction.Default);
            }
        }
#endregion
    }
}
