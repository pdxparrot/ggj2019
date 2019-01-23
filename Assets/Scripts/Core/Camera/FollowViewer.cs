using UnityEngine;

namespace pdxpartyparrot.Core.Camera
{
    [RequireComponent(typeof(FollowCamera))]
    public abstract class FollowViewer : Viewer
    {
        public FollowCamera FollowCamera { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            FollowCamera = GetComponent<FollowCamera>();
        }
#endregion
    }
}
