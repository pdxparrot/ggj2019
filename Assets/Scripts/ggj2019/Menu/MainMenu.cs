using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class MainMenu : Game.Menu.MainMenu
    {
        protected override bool UseMultiplayer => false;

#region Event Handlers
        public void OnStart()
        {
            //Owner.PushPanel(_gameTypePanel);
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.FFAGameStatePrefab);
        }
#endregion
    }
}
