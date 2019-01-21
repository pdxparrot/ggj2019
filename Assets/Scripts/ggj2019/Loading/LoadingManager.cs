using pdxpartyparrot.Game.Loading;

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
#endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();

            GameManager.CreateFromPrefab(_gameManagerPrefab, ManagersContainer);
        }
    }
}
