using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Loading
{
    public sealed class LoadingManager : Game.Loading.LoadingManager<LoadingManager>
    {
        [Space(10)]

#region Manager Prefabs
        [Header("Game Manager Prefabs")]

        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;

        [SerializeField]
        private NPCSpawner _npcSpawnerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
            PlayerManager.CreateFromPrefab(_playerManagerPrefab, ManagersContainer);
            NPCSpawner.CreateFromPrefab(_npcSpawnerPrefab, ManagersContainer);
        }
    }
}
