#pragma warning disable 0618    // disable obsolete warning for now

using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
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
    public abstract class Player2D : Actor2D, IPlayer
    {
        public GameObject GameObject => gameObject;

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

        protected PlayerDriver PlayerDriver => _driver;

        public virtual PlayerController2D PlayerBehavior => (PlayerController2D)Behavior;
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

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerController2D);
        }

        protected virtual void OnDestroy()
        {
            if(null != Viewer && ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(Viewer);
            }
            PlayerViewer = null;
        }
#endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            PlayerBehavior.Initialize();
            InitializeLocalPlayer(id);
        }

        protected virtual bool InitializeLocalPlayer(Guid id)
        {
            if(!IsLocalActor) {
                return false;
            }

            Debug.Log($"Initializing local player {id}");

            _driver.Initialize();

            NetworkPlayer.NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody2D;
            NetworkPlayer.NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisZ;

            return true;
        }

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            Debug.Log($"Spawning player (controller={NetworkPlayer.playerControllerId}, isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }

            Initialize(Guid.NewGuid());
            if(!NetworkClient.active) {
                NetworkPlayer.RpcSpawn(Id.ToString());
            }

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            Debug.Log($"Respawning player (controller={NetworkPlayer.playerControllerId}, isLocalPlayer={IsLocalActor})");

            if(NetworkServer.active) {
                NetworkPlayer.ResetPlayer(GameStateManager.Instance.PlayerManager.PlayerData);
            }

            return true;
        }
    }
}
