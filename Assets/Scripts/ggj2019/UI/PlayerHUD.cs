using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    [RequireComponent(typeof(UIObject))]
    public sealed class PlayerHUD : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;

#region Unity Lifecycle
        private void Update()
        {
            _scoreText.text = $"{GameManager.Instance.Score}";
        }
#endregion
    }
}
