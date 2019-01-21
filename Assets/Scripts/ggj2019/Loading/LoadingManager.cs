using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.State;
using UnityEngine;

namespace pdxpartyparrot.ggj2019.Loading
{
    public sealed class LoadingManager : Game.Loading.LoadingManager<LoadingManager>
    {
        [Space(10)]

#region Manager Prefabs
        [Header("Game Prefabs")]

        [SerializeField]
        private GameManager _gameManagerPrefab;

        [SerializeField]
        private PlayerManager _playerManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
            PlayerManager.CreateFromPrefab(_playerManagerPrefab, ManagersContainer);
        }

        protected override void InitializeManagers()
        {
            GameStateManager.Instance.RegisterGameManager(GameManager.Instance);
            GameStateManager.Instance.RegisterPlayerManager(PlayerManager.Instance);
        }
    }
}
