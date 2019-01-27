#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;

using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.Input;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(FollowTarget3D))]
    public sealed class ServerSpectator : MonoBehaviour, IServerSpectatorActions
    {
        private const string InvertLookYKey = "serverspectator.invertlooky";

        private bool InvertLookY
        {
            get => PartyParrotManager.Instance.GetBool(InvertLookYKey);
            set => PartyParrotManager.Instance.SetBool(InvertLookYKey, value);
        }

        [SerializeField]
        private float _mouseSensitivity = 0.5f;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerMove;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerLook;

        public FollowTarget3D FollowTarget { get; private set; }

        [CanBeNull]
        private ServerSpectatorViewer _viewer;

        [SerializeField]
        private ServerSpectatorControls _controls;

#region Unity Lifecycle
        private void Awake()
        {
            FollowTarget = GetComponent<FollowTarget3D>();

            _viewer = ViewerManager.Instance.AcquireViewer<ServerSpectatorViewer>();
            if(null != _viewer) {
                _viewer.Initialize(this);
            }

            _controls.MakePrivateCopyOfActions();
            _controls.ServerSpectator.SetCallbacks(this);
        }

        private void OnEnable()
        {
            _controls.ServerSpectator.Enable();
        }

        private void OnDisable()
        {
            _controls.ServerSpectator.Disable();
        }

        private void OnDestroy()
        {
            _controls.ServerSpectator.SetCallbacks(null);

            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(_viewer);
            }
            _viewer = null;
        }

        private void FixedUpdate()
        {
            float dt = Time.deltaTime;

            FollowTarget.LastLookAxes = Vector3.Lerp(FollowTarget.LastLookAxes, _lastControllerLook, dt * 20.0f);

            Quaternion rotation = null != _viewer ? Quaternion.AngleAxis(_viewer.transform.localEulerAngles.y, Vector3.up) : transform.rotation;
            transform.position = Vector3.Lerp(transform.position, transform.position + (rotation * _lastControllerMove), dt * 20.0f);
        }
#endregion

        private bool IsOurDevice(InputAction.CallbackContext ctx)
        {
            // no input unless we have focus
            if(!Application.isFocused) {
                return false;
            }

            // TODO: this probably doesn't handle multiple keyboards/mice
            return Keyboard.current == ctx.control.device || Mouse.current == ctx.control.device;
        }

#region IServerSpectatorActions
        public void OnMoveforward(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, context.started ? 1.0f : 0.0f);
        }

        public void OnMovebackward(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, _lastControllerMove.y, context.started ? -1.0f : 0.0f);
        }

        public void OnMoveleft(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(context.started ? -1.0f : 0.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        public void OnMoveright(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(context.started ? 1.0f : 0.0f, _lastControllerMove.y, _lastControllerMove.z);
        }

        public void OnMoveup(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, context.started ? 1.0f : 0.0f, _lastControllerMove.z);
        }

        public void OnMovedown(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            _lastControllerMove = new Vector3(_lastControllerMove.x, context.started ? -1.0f : 0.0f, _lastControllerMove.z);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if(!IsOurDevice(context)) {
                return;
            }

            if(context.cancelled) {
                _lastControllerLook = Vector3.zero;

                FollowTarget.LastLookAxes = _lastControllerLook;
            } else {
                Vector2 axes = context.ReadValue<Vector2>();
                axes.y *= InvertLookY ? -1 : 1;

                bool isMouse = context.control.device is Mouse;
                if(isMouse) {
                    axes *= _mouseSensitivity;
                }

                _lastControllerLook = new Vector3(axes.x, axes.y, 0.0f);
            }
        }
#endregion
    }
}
