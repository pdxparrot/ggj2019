using pdxpartyparrot.ggj2019.State;
using pdxpartyparrot.Game.Loading;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Loading
{
    public sealed class LoadingManager : LoadingManager<LoadingManager>
    {
#region Manager Prefabs
        [SerializeField]
        private GameStateManager _gameStateManagerPrefab;
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameStateManager.CreateFromPrefab(_gameStateManagerPrefab, ManagersContainer);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GameStateManager.Instance.TransitionToInitialState();
        }
    }
}
