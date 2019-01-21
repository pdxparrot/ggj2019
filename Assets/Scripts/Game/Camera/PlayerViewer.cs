using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Players;

namespace pdxpartyparrot.Game.Camera
{
    public interface IPlayerViewer
    {
        Viewer Viewer { get; }

        void Initialize(Player target, int id);
    }
}
