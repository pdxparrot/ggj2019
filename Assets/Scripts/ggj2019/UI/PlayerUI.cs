using pdxpartyparrot.Core.Util;
using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerUI : Game.UI.PlayerUI
    {
        [SerializeField]
        private GameObject _deathText;

        [SerializeField]
        private TextMeshProUGUI _endGameScoreText;

        [SerializeField]
        private GameObject _gameOverText;

        [SerializeField]
        private TextMeshProUGUI _waveText;

        [SerializeField]
        private GameObject _waveTextObject;

        [SerializeField]
        private float _waveTextTime = 2.5f;

        private readonly Timer _waveTextTimer = new Timer();

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            _waveTextTimer.Update(dt);
        }
#endregion

        public override void Initialize(UnityEngine.Camera uiCamera)
        {
            base.Initialize(uiCamera);

            ShowDeathText(false);
            ShowGameOver(false);
            _waveTextObject.SetActive(false);
        }

        public void ShowDeathText(bool show)
        {
            _deathText.SetActive(show);
        }

        public void ShowGameOver(bool show)
        {
            _gameOverText.SetActive(show);
            if(show) {
                ShowDeathText(false);
                _waveTextObject.SetActive(false);
                _waveTextTimer.Stop();
            }
        }

        public void SetScoreText(int score)
        {
            _endGameScoreText.text = $"Your Score: {score}";
        }

        public void ShowWaveText(int wave)
        {
            _waveText.text = $"Wave {wave} incoming! Get Ready!";
            _waveTextObject.SetActive(true);
            _waveTextTimer.Start(_waveTextTime, () => {
                _waveTextObject.SetActive(false);
            });
        }
    }
}
