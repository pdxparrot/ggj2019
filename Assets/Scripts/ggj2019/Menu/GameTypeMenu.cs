using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class GameTypeMenu : MenuPanel
    {
#region Event Handlers
        public void OnFreeForAll()
        {
            GameStateManager.Instance.StartSinglePlayer();
        }

        public void OnTeams()
        {
            GameStateManager.Instance.StartSinglePlayer();
        }

        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
