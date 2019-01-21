using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public static class UnityUtil
    {
        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
