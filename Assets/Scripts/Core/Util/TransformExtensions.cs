using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public static class TransformExtensions
    {
        // http://answers.unity3d.com/questions/654222/make-sprite-look-at-vector2-in-unity-2d-1.html
        public static void LookAt2D(this Transform transform, Vector3 target)
        {
            Vector3 dir = (target - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void LookAt2D(this Transform transform, Transform target)
        {
            if(null == target) {
                return;
            }

            transform.LookAt2D(target.position);
        }

        public static void Clear(this Transform transform)
        {
            foreach(Transform child in transform) {
                Object.Destroy(child.gameObject);
            }
        }
    }
}
