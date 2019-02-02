using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.Loading
{
    public abstract class LoadingManager<T> : Core.Loading.LoadingManager<LoadingManager<T>> where T: LoadingManager<T>
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
            HighScoreManager.Create(ManagersContainer);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GameStateManager.Instance.TransitionToInitialState();
        }
    }
}
