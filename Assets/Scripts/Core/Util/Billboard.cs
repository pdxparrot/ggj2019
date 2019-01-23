using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public class Billboard : MonoBehaviour
    {
        [CanBeNull]
        public UnityEngine.Camera Camera { get; set; }

#region Unity Lifecycle
        private void LateUpdate()
        {
            Transform ctransform = transform;
            if(null != Camera) {
                ctransform.forward = (Camera.transform.position - ctransform.position).normalized;
            }
        }
#endregion
    }
}
