using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Data;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerManager : Game.Players.PlayerManager<PlayerManager>
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
