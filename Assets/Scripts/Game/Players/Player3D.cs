#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(NetworkPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Player3D : PhysicsActor3D, IPlayer
    {
#region Network
        public override bool IsLocalActor => NetworkPlayer.isLocalPlayer;

        // need this to hand off to the NetworkManager before instantiating
        [SerializeField]
        private NetworkPlayer _networkPlayer;

        public NetworkPlayer NetworkPlayer => _networkPlayer;
#endregion

#region Controller / Driver
        [SerializeField]
        private PlayerDriver _driver;

        public virtual PlayerController3D PlayerController => (PlayerController3D)Controller;
#endregion

#region Viewer

        [CanBeNull]
        public IPlayerViewer PlayerViewer { get; private set; }

        [CanBeNull]
        public override Viewer Viewer
        {
            get => null == PlayerViewer ? null : PlayerViewer.Viewer;
            set {}
        }
#endregion

        private AudioSource _audioSource;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Controller is PlayerController3D);

            _audioSource = GetComponent<AudioSource>();
            AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);

            GameStateManager.Instance.PlayerManager.Register(this);
        }

        protected override void OnDestroy()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(Viewer);
            }
            PlayerViewer = null;

            if(GameStateManager.HasInstance && GameStateManager.Instance.HasPlayerManager) {
                GameStateManager.Instance.PlayerManager.Unregister(this);
            }

            base.OnDestroy();
        }
#endregion

        public override void Initialize(int id)
        {
            base.Initialize(id);

            InitializeLocalPlayer(id);
        }

        protected virtual bool InitializeLocalPlayer(int id)
        {
            if(!IsLocalActor) {
                return false;
            }

            Debug.Log("Initializing local player");

            _driver.Initialize();

            PlayerViewer = ViewerManager.Instance.AcquireViewer<Viewer>() as IPlayerViewer;
            if(null == PlayerViewer) {
                Debug.LogError("Unable to acquire player viewer!");
            } else {
                PlayerViewer.Initialize(this, id);
            }

            UIManager.Instance.InitializePlayerUI(this);

            return true;
        }

        public override void OnSpawn()
        {
            Debug.Log($"Spawning player (isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }

            Initialize(NetworkPlayer.playerControllerId);
            if(!NetworkClient.active) {
                NetworkPlayer.RpcSpawn(NetworkPlayer.playerControllerId);
            }
        }

        public override void OnReSpawn()
        {
            Debug.Log($"Respawning player (isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }
        }
    }
}
