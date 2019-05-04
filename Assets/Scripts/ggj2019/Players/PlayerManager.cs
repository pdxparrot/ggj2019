using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerManager : Game.Players.PlayerManager<PlayerManager, Player>
    {
#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            GameStateManager.Instance.RegisterPlayerManager(this);
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterPlayerManager();
            }

            base.OnDestroy();
        }
#endregion
    }
}
