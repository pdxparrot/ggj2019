using pdxpartyparrot.Game.Players;

namespace pdxpartyparrot.ggj2019.Players.ControllerComponents
{
    public abstract class GamePlayerControllerComponent : PlayerControllerComponent2D
    {
        protected Player GamePlayer => (Player)PlayerBehavior.Player;
    }
}
