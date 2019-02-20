using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    [RequireComponent(typeof(UIObject))]
    public sealed class NewWaveUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _waveText;

#region Unity Lifecycle
        private void OnEnable()
        {
            _waveText.text = $"Wave {GameManager.Instance.WaveSpawner.CurrentWaveIndex} Incoming! Get Ready!";
        }
#endregion
    }
}
