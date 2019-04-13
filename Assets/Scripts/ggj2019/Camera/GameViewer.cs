using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.Characters.Players;

namespace pdxpartyparrot.ggj2019.Camera
{
    public sealed class GameViewer : StaticViewer, IPlayerViewer
    {
        public Viewer Viewer => this;

        public void Initialize(IPlayer player, int id)
        {
            // nothing to do here
        }
    }
}
