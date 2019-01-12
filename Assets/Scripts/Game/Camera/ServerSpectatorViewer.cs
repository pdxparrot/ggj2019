using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.Actors;

namespace pdxpartyparrot.Game.Camera
{
    public sealed class ServerSpectatorViewer : FollowViewer
    {
        public void Initialize(ServerSpectator owner)
        {
            Initialize(0);

            Set3D();

            FollowCamera.SetTarget(owner.FollowTarget);
            SetFocus(owner.transform);
        }
    }
}
