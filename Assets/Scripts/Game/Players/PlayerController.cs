using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerController
    {
        PlayerBehaviorData PlayerBehaviorData { get; }

        IPlayer Player { get; }
    }
}
