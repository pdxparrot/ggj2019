using pdxpartyparrot.ggj2019.Loading;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameStateManager : GameStateManager<GameStateManager>
    {
        protected override void ShowLoadingScreen(bool show)
        {
            LoadingManager.Instance.ShowLoadingScreen(show);
        }

        protected override void UpdateLoadingScreen(float percent, string text)
        {
            LoadingManager.Instance.UpdateLoadingScreen(percent, text);
        }
    }
}