using pdxpartyparrot.Game.State;
using UnityEngine;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class MainMenu : Game.Menu.MainMenu
    {
        [SerializeField]
        private GameTypeMenu _gameTypePanel;

        protected override bool UseMultiplayer => false;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            if(null != _gameTypePanel) {
                _gameTypePanel.gameObject.SetActive(false);
            }
        }
#endregion

#region Event Handlers
        public void OnStart()
        {
            //Owner.PushPanel(_gameTypePanel);
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.FFAGameStatePrefab);
        }
#endregion
    }
}
