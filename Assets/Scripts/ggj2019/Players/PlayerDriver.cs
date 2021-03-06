using System;

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
    public sealed class PlayerDriver : Game.Players.PlayerDriver, PlayerControls.IPlayerActions, IPauseActionHandler
    {
        private PlayerControls _controls;

        private Player GamePlayer => (Player)Player;

        public GamepadListener GamepadListener { get; private set; }

        protected override bool CanDrive => base.CanDrive && !GamePlayer.IsDead && !GameManager.Instance.IsGameOver;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Player is Player);
            Assert.IsNull(GetComponent<GamepadListener>());

            _controls = new PlayerControls();

            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected override void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }

            _controls.Player.Disable();
            _controls.Player.SetCallbacks(null);

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

            if(PlayerManager.Instance.DebugInput) {
                Debug.Log($"Pause: {context.action.phase}");
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
                GamePlayer.PlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMovedown(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(LastControllerMove.x, context.performed ? -1.0f : 0.0f, 0.0f);
            if(context.cancelled) {
                GamePlayer.PlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMoveleft(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.performed ? -1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                GamePlayer.PlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }

        public void OnMoveright(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            LastControllerMove = new Vector3(context.performed ? 1.0f : 0.0f, LastControllerMove.y, 0.0f);
            if(context.cancelled) {
                GamePlayer.PlayerBehavior.SetMoveDirection(LastControllerMove);
            }
        }
#endregion

        public void OnGather(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(PlayerManager.Instance.DebugInput) {
                Debug.Log($"Gather: {context.action.phase}");
            }

            if(context.performed) {
                GamePlayer.GamePlayerBehavior.ActionPerformed(GatherBehaviorComponent.GatherAction.Default);
            }
        }

        public void OnContext(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context) || !CanDrive) {
                return;
            }

            if(PlayerManager.Instance.DebugInput) {
                Debug.Log($"Context: {context.action.phase}");
            }

            if(context.performed) {
                GamePlayer.GamePlayerBehavior.ActionPerformed(ContextBehaviorComponent.ContextAction.Default);
            }
        }
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(PartyParrotManager.Instance.IsPaused) {
                if(InputManager.Instance.DebugInput) {
                    Debug.Log("Disabling player controls");
                }
                _controls.Disable();
            } else {
                if(InputManager.Instance.DebugInput) {
                    Debug.Log("Enabling player controls");
                }
                _controls.Enable();
            }
        }
#endregion
    }
}
