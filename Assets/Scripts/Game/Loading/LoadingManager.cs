using System.Collections.Generic;

using pdxpartyparrot.Core.Loading;
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

        protected override IEnumerator<LoadStatus> OnLoadRoutine()
        {
            IEnumerator<LoadStatus> runner = base.OnLoadRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            GameStateManager.Instance.TransitionToInitialStateAsync();
        }
    }
}
