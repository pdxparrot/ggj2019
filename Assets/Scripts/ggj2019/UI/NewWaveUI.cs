using pdxpartyparrot.Game.UI;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class NewWaveUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _waveText;

#region Unity Lifecycle
        private void Awake()
        {
            UIManager.Instance.RegisterUIObject("wave_text", gameObject);
        }

        private void OnDestroy()
        {
            if(UIManager.HasInstance) {
                UIManager.Instance.UnregisterUIObject("wave_text");
            }
        }

        private void OnEnable()
        {
            _waveText.text = $"Wave {GameManager.Instance.WaveSpawner.CurrentWaveIndex} Incoming! Get Ready!";
        }
#endregion
    }
}
