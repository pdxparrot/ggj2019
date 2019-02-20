using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    [RequireComponent(typeof(UIObject))]
    public sealed class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _endGameWaveText;

        [SerializeField]
        private TextMeshProUGUI _endGameScoreText;

#region Unity Lifecycle
        private void OnEnable()
        {
            _endGameWaveText.text = $"You Made It To Wave {GameManager.Instance.WaveSpawner.CurrentWaveIndex}!";
            _endGameScoreText.text = $"Your Score: {GameManager.Instance.Score}";
        }
#endregion
    }
}
