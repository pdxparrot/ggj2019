using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Actors;

namespace pdxpartyparrot.Game.Camera
{
    public sealed class ServerSpectatorViewer : FollowViewer3D
    {
        public void Initialize(ServerSpectator owner)
        {
            Initialize(0);

            FollowCamera3D.SetTarget(owner.FollowTarget);
            SetFocus(owner.transform);
        }
    }
}
