using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Data;

namespace pdxpartyparrot.ggj2019.Players
{
    // TODO: find a way to make this unnecessary (all it's for is casting the GamePlayerData)
    public sealed class PlayerManager : Game.Players.PlayerManager<PlayerManager, Player>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;

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
