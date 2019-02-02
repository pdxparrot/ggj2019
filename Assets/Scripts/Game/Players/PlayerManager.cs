#pragma warning disable 0618    // disable obsolete warning for now

using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerManager
    {
        IReadOnlyCollection<Actor> Actors { get; }

        PlayerData PlayerData { get; }

        int PlayerCount { get; }

        void Register(Actor player);

        void Unregister(Actor player);
    }

    public abstract class PlayerManager<T> : ActorManager<Actor, T>, IPlayerManager where T: PlayerManager<T>
    {
#region Data
        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
#endregion

        [Space(10)]

        [SerializeField]
        private Actor _playerPrefab;

        private IPlayer PlayerPrefab => (IPlayer)_playerPrefab;

        public int PlayerCount => ActorCount;

        private GameObject _playerContainer;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Assert.IsTrue(_playerPrefab is IPlayer);

            _playerContainer = new GameObject("Players");

            Core.Network.NetworkManager.Instance.RegisterPlayerPrefab(PlayerPrefab.NetworkPlayer);

            Core.Network.NetworkManager.Instance.ServerAddPlayerEvent += ServerAddPlayerEventHandler;

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(Core.Network.NetworkManager.HasInstance) {
                Core.Network.NetworkManager.Instance.ServerAddPlayerEvent -= ServerAddPlayerEventHandler;

                Core.Network.NetworkManager.Instance.UnregisterPlayerPrefab();
            }

            Destroy(_playerContainer);
            _playerContainer = null;

            DestroyDebugMenu();

            base.OnDestroy();
        }
#endregion

        private void SpawnPlayer(NetworkConnection conn, short controllerId)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Spawning player for controller {controllerId}...");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetPlayerSpawnPoint(controllerId);
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            NetworkPlayer player = Core.Network.NetworkManager.Instance.SpawnPlayer<NetworkPlayer>(controllerId, conn, _playerContainer.transform);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            spawnPoint.Spawn((Actor)player.Player);
        }

        public void RespawnPlayer(IPlayer player)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Respawning player {player.Name}");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetPlayerSpawnPoint(player.NetworkPlayer.playerControllerId);
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            spawnPoint.ReSpawn((Actor)player);
        }

#region Event Handlers
        private void ServerAddPlayerEventHandler(object sender, ServerAddPlayerEventArgs args)
        {
            SpawnPlayer(args.NetworkConnection, args.PlayerControllerId);
        }
#endregion

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Game.PlayerManager");
            _debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Players", GUI.skin.box);
                    foreach(IPlayer player in Actors) {
                        GUILayout.Label($"{player.Name} {player.Position}");
                    }
                GUILayout.EndVertical();
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
