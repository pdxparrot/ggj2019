using JetBrains.Annotations;

using UnityEngine.Profiling;

namespace pdxpartyparrot.Core.Camera
{
    public class FollowCamera2D : FollowCamera
    {
        [CanBeNull]
        public FollowTarget2D Target2D => (FollowTarget2D)Target;

        public void SetTarget(FollowTarget2D target)
        {
            Target = target;
        }

        protected override void HandleInput(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera2D.HandleInput");
            try {
                Zoom(dt);
            } finally {
                Profiler.EndSample();
            }
        }

        private void Zoom(float dt)
        {
            if(!EnableZoom) {
                return;
            }

            // TODO
        }

        protected override void FollowTarget(float dt)
        {
            if(null == Target) {
                return;
            }

            Profiler.BeginSample("FollowCamera3D.FollowTarget");
            try {
                // TODO
            } finally {
                Profiler.EndSample();
            }
        }
    }
}
