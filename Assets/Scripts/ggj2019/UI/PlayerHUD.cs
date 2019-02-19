using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;

#region Unity Lifecycle
        private void Awake()
        {
            UIManager.Instance.RegisterUIObject("player_hud", gameObject);
        }

        private void OnDestroy()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.UnregisterUIObject("player_hud");
            }
        }

        private void Update()
        {
            _scoreText.text = $"{GameManager.Instance.Score}";
        }
#endregion
    }
}
