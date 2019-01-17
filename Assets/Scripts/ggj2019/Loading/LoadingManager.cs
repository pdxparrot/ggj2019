using pdxpartyparrot.Game.Loading;
using pdxpartyparrot.ggj2019.State;
using pdxpartyparrot.ggj2019.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private GameStateManager _gameStateManagerPrefab;

        [SerializeField]
        private UIManager _uiManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameStateManager.CreateFromPrefab(_gameStateManagerPrefab, ManagersContainer);
            UIManager.CreateFromPrefab(_uiManagerPrefab, ManagersContainer);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GameStateManager.Instance.TransitionToInitialState();
        }
    }
}
