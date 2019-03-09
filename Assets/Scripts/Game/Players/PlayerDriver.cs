using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerDriver : CharacterDriver
    {
        [SerializeField]
        private PlayerDriverData _data;

        [SerializeField]
        private float _mouseSensitivity = 0.5f;

        protected float MouseSensitivity => _mouseSensitivity;

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

        protected IPlayerBehavior PlayerBehavior => (IPlayerBehavior)CharacterBehavior;

        protected IPlayer Player => PlayerBehavior.Player;

        protected override bool CanDrive => base.CanDrive && Player.IsLocalActor;

        protected bool EnableMouseLook { get; private set; } = !Application.isEditor;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(CharacterBehavior is IPlayerBehavior);
        }

        protected virtual void Update()
        {
            if(!Player.IsLocalActor) {
                return;
            }

            if(!CanDrive) {
                // TODO: on pause tho we should maybe store this stuff out
                // in order to reset it (otherwise we might not get new inputs)
                LastControllerMove = Vector3.zero;
                Behavior.LastMoveAxes = Vector3.zero;
                return;
            }


            float dt = Time.deltaTime;

            Behavior.LastMoveAxes = Vector3.Lerp(Behavior.LastMoveAxes, _lastControllerMove, dt * _data.MovementLerpSpeed);
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
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Game.Player {Player.Id} Driver");
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
