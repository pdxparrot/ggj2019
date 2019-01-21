#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(NetworkPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Player : Actor
    {
#region Network
        public override bool IsLocalActor => NetworkPlayer.isLocalPlayer;

        [SerializeField]
        private NetworkPlayer _networkPlayer;

        public NetworkPlayer NetworkPlayer => _networkPlayer;
#endregion

#region Controller / Driver
        [SerializeField]
        private PlayerDriver _driver;

        public virtual PlayerController PlayerController => (PlayerController)Controller;
#endregion

#region Viewer
        [CanBeNull]
        private IPlayerViewer _viewer;

        public IPlayerViewer PlayerViewer => _viewer;

        [CanBeNull]
        public override Viewer Viewer => _viewer?.Viewer;
#endregion

        private AudioSource _audioSource;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if(!(Controller is PlayerController)) {
                Debug.LogError("Player controller must be a PlayerController!");
            }
#endif

            _audioSource = GetComponent<AudioSource>();
            AudioManager.Instance.InitSFXAudioMixerGroup(_audioSource);

Debug.Log("TODO: register player");
//            PlayerManager.Instance.Register(this);
        }

        protected override void OnDestroy()
        {
            if(ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(Viewer);
            }
            _viewer = null;

Debug.Log("TODO: unregister player");
            /*if(PlayerManager.HasInstance) {
                PlayerManager.Instance.Unregister(this);
            }*/

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

            // TODO: this is gross
            _viewer = ViewerManager.Instance.AcquireViewer<Viewer>() as IPlayerViewer;
            _viewer?.Initialize(this, id);

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
