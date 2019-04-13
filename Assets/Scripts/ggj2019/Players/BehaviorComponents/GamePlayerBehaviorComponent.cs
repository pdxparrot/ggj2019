using pdxpartyparrot.Game.Characters.Players.BehaviorComponents;

namespace pdxpartyparrot.ggj2019.Players.BehaviorComponents
{
    public abstract class GamePlayerBehaviorComponent : PlayerBehaviorComponent2D
    {
        protected Player GamePlayer => (Player)PlayerBehavior.Player;
    }
}
