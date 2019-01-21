using pdxpartyparrot.ggj2019.Data;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerManager : Game.Players.PlayerManager<Player, PlayerManager>
    {
        public PlayerData GamePlayerData => (PlayerData)PlayerData;
    }
}
