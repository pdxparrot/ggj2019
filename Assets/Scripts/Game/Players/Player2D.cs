#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(NetworkPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Player2D : PhysicsActor2D, IPlayer
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

        public virtual PlayerController2D PlayerController => (PlayerController2D)Controller;
#endregion

#region Viewer

        [CanBeNull]
        public IPlayerViewer PlayerViewer { get; protected set; }

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

            Assert.IsTrue(Controller is PlayerController2D);

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

            PlayerController.Initialize();
            InitializeLocalPlayer(id);
        }

        protected virtual bool InitializeLocalPlayer(int id)
        {
            if(!IsLocalActor) {
                return false;
            }

            Debug.Log("Initializing local player");

            _driver.Initialize();

            NetworkPlayer.NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody2D;
            NetworkPlayer.NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisZ;

            InitializeViewer();
            InitializeModel();
            InitializePlayerUI();

            return true;
        }

        protected abstract void InitializeViewer();

        protected virtual void InitializeModel()
        {
        }

        protected virtual void InitializePlayerUI()
        {
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            Debug.Log($"Spawning player (controller={NetworkPlayer.playerControllerId}, isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }

            Initialize(NetworkPlayer.playerControllerId);
            if(!NetworkClient.active) {
                NetworkPlayer.RpcSpawn(NetworkPlayer.playerControllerId);
            }
        }

        public override void OnReSpawn(SpawnPoint spawnpoint)
        {
            base.OnReSpawn(spawnpoint);

            Debug.Log($"Respawning player (controller={NetworkPlayer.playerControllerId}, isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }
        }
    }
}
