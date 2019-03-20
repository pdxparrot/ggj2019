using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerBehavior
    {
        PlayerBehaviorData PlayerBehaviorData { get; }

        IPlayer Player { get; }
    }
}
