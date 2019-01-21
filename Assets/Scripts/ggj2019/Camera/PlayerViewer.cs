using pdxpartyparrot.Core.Camera;

namespace pdxpartyparrot.ggj2019.Camera
{
    public sealed class PlayerViewer : FollowViewer
    {
        public void Initialize(/*Player target*/)
        {
            Initialize(0);

            Set3D();

//            FollowCamera.SetTarget(_target.FollowTarget);
//            SetFocus(_target.transform);
        }
    }
}
