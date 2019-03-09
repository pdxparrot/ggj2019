#pragma warning disable 0618    // disable obsolete warning for now

using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Camera;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    // TODO: reduce the copy paste in this
    [RequireComponent(typeof(NetworkPlayer))]
    public abstract class Player3D : Actor3D, IPlayer
    {
        public GameObject GameObject => gameObject;

#region Network
        public override bool IsLocalActor => NetworkPlayer.isLocalPlayer;

        // need this to hand off to the NetworkManager before instantiating
        [SerializeField]
        private NetworkPlayer _networkPlayer;

        public NetworkPlayer NetworkPlayer => _networkPlayer;
#endregion

#region Behavior / Driver
        [SerializeField]
        private PlayerDriver _driver;

        public PlayerDriver PlayerDriver => _driver;

        [CanBeNull]
        public IPlayerBehavior PlayerBehavior => (PlayerBehavior3D)Behavior;
#endregion

#region Viewer

        [CanBeNull]
        public IPlayerViewer PlayerViewer { get; protected set; }

        [CanBeNull]
        public Viewer Viewer
        {
            get => null == PlayerViewer ? null : PlayerViewer.Viewer;
            set {}
        }
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PlayerBehavior3D);
        }

        protected override void OnDestroy()
        {
            if(null != Viewer && ViewerManager.HasInstance) {
                ViewerManager.Instance.ReleaseViewer(Viewer);
            }
            PlayerViewer = null;

            base.OnDestroy();
        }
#endregion

        public override void Initialize(Guid id)
        {
            base.Initialize(id);

            InitializeLocalPlayer(id);
            if(null != PlayerBehavior) {
                PlayerBehavior.Initialize();
            }
        }

        protected virtual bool InitializeLocalPlayer(Guid id)
        {
            if(!IsLocalActor) {
                return false;
            }

            Debug.Log($"Initializing local player {id}");

            _driver.Initialize();

            NetworkPlayer.NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkPlayer.NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;

            return true;
        }

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            Debug.Log($"Spawning player (controller={NetworkPlayer.playerControllerId}, isLocalPlayer={IsLocalActor})");

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

            return true;
        }
    }
}
