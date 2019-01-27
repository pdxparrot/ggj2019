namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerController : Game.Players.PlayerController2D
    {
        public Data.PlayerControllerData GamePlayerControllerData => (Data.PlayerControllerData)PlayerControllerData;

        public Player GamePlayer => (Player)Player;
    }
}
