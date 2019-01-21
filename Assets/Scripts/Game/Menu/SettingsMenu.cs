namespace pdxpartyparrot.Game.Menu
{
    public sealed class SettingsMenu : MenuPanel
    {
#region Event Handlers
        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
