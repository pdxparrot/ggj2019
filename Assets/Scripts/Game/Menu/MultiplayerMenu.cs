using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class MultiplayerMenu : MenuPanel
    {
#region Event Handlers
        public void OnHost()
        {
            GameStateManager.Instance.StartHost();
        }

        public void OnJoin()
        {
            GameStateManager.Instance.StartJoin();
        }

        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
