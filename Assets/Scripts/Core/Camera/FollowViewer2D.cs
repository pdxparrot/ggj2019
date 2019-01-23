using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(FollowCamera2D))]
    public class FollowViewer2D : FollowViewer
    {
        public FollowCamera2D FollowCamera2D => (FollowCamera2D)FollowCamera;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(FollowCamera is FollowCamera2D);

            Set2D();
        }
#endregion
    }
}
