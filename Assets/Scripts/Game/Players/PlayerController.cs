using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayerController
    {
        PlayerControllerData PlayerControllerData { get; }

        IPlayer Player { get; }
    }
}
