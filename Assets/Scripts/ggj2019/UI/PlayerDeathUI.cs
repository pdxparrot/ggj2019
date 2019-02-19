using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerDeathUI : MonoBehaviour
    {
#region Unity Lifecycle
        private void Awake()
        {
            UIManager.Instance.RegisterUIObject("player_death_text", gameObject);
        }

        private void OnDestroy()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.UnregisterUIObject("player_death_text");
            }
        }
#endregion
    }
}
