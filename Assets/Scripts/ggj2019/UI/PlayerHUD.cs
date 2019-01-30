using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
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
