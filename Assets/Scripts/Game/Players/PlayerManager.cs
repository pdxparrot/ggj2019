#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.World;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerManager
    {
        PlayerData PlayerData { get; }
    }

    public abstract class PlayerManager<T, TV> : ActorManager<T, TV>, IPlayerManager where T: Player where TV: PlayerManager<T, TV>
    {
#region Data
        [SerializeField]
        private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
#endregion

        [Space(10)]

        [SerializeField]
        private T _playerPrefab;

        private GameObject _playerContainer;

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            _playerContainer = new GameObject("Players");

            Core.Network.NetworkManager.Instance.RegisterPlayerPrefab(_playerPrefab.NetworkPlayer);

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

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            NetworkPlayer player = Core.Network.NetworkManager.Instance.SpawnPlayer<NetworkPlayer>(controllerId, conn, _playerContainer.transform);
            if(null == player) {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            spawnPoint.Spawn(player.Player);
        }

        public void RespawnPlayer(T player)
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.Log($"Respawning player {player.name}");

            SpawnPoint spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            if(null == spawnPoint) {
                Debug.LogError("Failed to get player spawnpoint!");
                return;
            }

            spawnPoint.ReSpawn(player);
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
                    foreach(T player in Actors) {
                        GUILayout.Label($"{player.name} {player.transform.position}");
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
