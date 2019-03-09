using pdxpartyparrot.Game.Players;

namespace pdxpartyparrot.ggj2019.Players.ControllerComponents
{
    public abstract class GamePlayerControllerComponent : PlayerBehaviorComponent2D
    {
        protected Player GamePlayer => (Player)PlayerBehavior.Player;
    }
}
