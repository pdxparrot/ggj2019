using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.State;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class GameTypeMenu : MenuPanel
    {
#region Event Handlers
        public void OnFreeForAll()
        {
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.FFAGameStatePrefab);
        }

        public void OnTeams()
        {
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.TeamsGameStatePrefab);
        }

        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
