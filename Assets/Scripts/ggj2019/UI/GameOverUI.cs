using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _endGameWaveText;

        [SerializeField]
        private TextMeshProUGUI _endGameScoreText;

#region Unity Lifecycle
        private void Awake()
        {
            UIManager.Instance.RegisterUIObject("game_over_text", gameObject);
        }

        private void OnDestroy()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.UnregisterUIObject("game_over_text");
            }
        }

        private void OnEnable()
        {
            _endGameWaveText.text = $"You Made It To Wave {GameManager.Instance.WaveSpawner.CurrentWaveIndex}!";
            _endGameScoreText.text = $"Your Score: {GameManager.Instance.Score}";
        }
#endregion
    }
}
