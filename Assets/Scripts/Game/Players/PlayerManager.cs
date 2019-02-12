#pragma warning disable 0618    // disable obsolete warning for now

using System.Collections.Generic;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    // TODO: find a way to kill this
    public interface IPlayerManager
    {
        PlayerData PlayerData { get; }

        IReadOnlyCollection<IPlayer> Players { get; }

        int PlayerCount { get; }
    }

    public abstract class PlayerManager<T, TV> : SingletonBehavior<T>, IPlayerManager where T: PlayerManager<T, TV> where TV: Actor, IPlayer
    {
#region Data
        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
#endregion

        [Space(10)]

        [SerializeField]
        private TV _playerPrefab;

        private TV PlayerPrefab => _playerPrefab;

        private readonly HashSet<IPlayer> _players = new HashSet<IPlayer>();

        public IReadOnlyCollection<IPlayer> Players => _players;

        public int PlayerCount => Players.Count;

        private GameObject _playerContainer;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        protected virtual void Awake()
        {
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

            spawnPoint.SpawnPlayer((Actor)player.Player);

            _players.Add(player.Player);
        }

        public bool RespawnPlayer(IPlayer player)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Respawning player {player.Id}");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetPlayerSpawnPoint(player.NetworkPlayer.playerControllerId);
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return false;
            }

            return spawnPoint.ReSpawn((Actor)player);
        }

        // TODO: figure out how to work this in when players disconnect
        public void DespawnPlayer(IPlayer player)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Despawning player {player.Id}");

            _players.Remove(player);
        }

        // TODO: figure out how to even do thid
        public void DespawnPlayers()
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Despawning {Players.Count} players...");

            // TODO: how?

            _players.Clear();
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
                    foreach(IPlayer player in _players) {
                        GUILayout.Label($"{player.Id} {player.GameObject.transform.position}");
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
