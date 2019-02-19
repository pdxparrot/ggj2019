using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class IntroUI : MonoBehaviour
    {
#region Unity Lifecycle
        private void Awake()
        {
            UIManager.Instance.RegisterUIObject("intro_text", gameObject);
        }

        private void OnDestroy()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.UnregisterUIObject("intro_text");
            }
        }
#endregion
    }
}
