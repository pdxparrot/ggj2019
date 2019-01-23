using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerDriver : ActorDriver
    {
        [SerializeField]
        private float _mouseSensitivity = 0.5f;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerMove;

        protected Vector3 LastControllerMove
        {
            get => _lastControllerMove;
            set => _lastControllerMove = value;
        }

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastControllerLook;

        protected Vector3 LastControllerLook
        {
            get => _lastControllerLook;
            set => _lastControllerLook = value;
        }

        protected IPlayerController PlayerController => (IPlayerController)Controller;

        protected IPlayer Player => PlayerController.Player;

        protected override bool CanDrive => base.CanDrive && Player.IsLocalActor;

        protected bool EnableMouseLook { get; private set; } = !Application.isEditor;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsTrue(Controller is IPlayerController);
        }

        protected virtual void Update()
        {
            if(!Player.IsLocalActor) {
                return;
            }

            float dt = Time.deltaTime;

            Controller.LastMoveAxes = Vector3.Lerp(Controller.LastMoveAxes, _lastControllerMove, dt * GameStateManager.Instance.PlayerManager.PlayerData.MovementLerpSpeed);
        }

        protected virtual void OnDestroy()
        {
            DestroyDebugMenu();
        }
#endregion

        public virtual void Initialize()
        {
            if(!Player.IsLocalActor) {
                return;
            }

            InitDebugMenu();
        }

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Game.Player {Player.Name} Driver");
            _debugMenuNode.RenderContentsAction = () => {
                /*GUILayout.BeginHorizontal();
                    GUILayout.Label("Mouse Sensitivity:");
                    _mouseSensitivity = GUIUtils.FloatField(_mouseSensitivity);
                GUILayout.EndHorizontal();*/

                if(Application.isEditor) {
                    EnableMouseLook = GUILayout.Toggle(EnableMouseLook, "Enable Mouse Look");
                }
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
