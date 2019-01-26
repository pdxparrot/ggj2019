using pdxpartyparrot.Game.Players;

namespace pdxpartyparrot.ggj2019.Player.ControllerComponents
{
    public abstract class GamePlayerControllerComponent : PlayerControllerComponent
    {
        protected Players.Player GamePlayer => (Players.Player)PlayerController.Player;
    }
}
