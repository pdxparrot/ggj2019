using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;

using Spine;
using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerUI : Game.UI.PlayerUI
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

#region Intro
        [SerializeField]
        private GameObject _introTextObject;

        [SerializeField]
        private float _introTextTime = 2.5f;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _introTextTimer = new Timer();
#endregion

#region Wave
        [SerializeField]
        private GameObject _waveTextObject;

        [SerializeField]
        private TextMeshProUGUI _waveText;

        [SerializeField]
        private float _waveTextTime = 2.5f;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _waveTextTimer = new Timer();
#endregion

#region Death
        [SerializeField]
        private GameObject _deathTextObject;
#endregion

#region End Game
        [SerializeField]
        private GameObject _gameOverTextObject;

        [SerializeField]
        private TextMeshProUGUI _endGameWaveText;

        [SerializeField]
        private TextMeshProUGUI _endGameScoreText;
#endregion

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            _introTextTimer.Update(dt);
            _waveTextTimer.Update(dt);
        }
#endregion

        public override void Initialize(UnityEngine.Camera uiCamera)
        {
            base.Initialize(uiCamera);

            ShowDeathText(false);
            ShowGameOver(false);
            _waveTextObject.SetActive(false);

            _introTextObject.SetActive(true);
            _introTextTimer.Start(_introTextTime, () => {
                _introTextObject.SetActive(false);
            });
        }

        public void ShowPlayerHUD(bool show)
        {
            _playerHUD.gameObject.SetActive(show);
        }

        public void ShowDeathText(bool show)
        {
            _deathTextObject.SetActive(show);
        }

        public void ShowGameOver(bool show)
        {
            _gameOverTextObject.SetActive(show);

            if(show) {
                _endGameWaveText.text = $"You Made It To Wave {GameManager.Instance.WaveSpawner.CurrentWaveIndex}!";
                _endGameScoreText.text = $"Your Score: {GameManager.Instance.Score}";

                ShowDeathText(false);

                _waveTextObject.SetActive(false);
                _waveTextTimer.Stop();
            }
        }

        public void ShowWaveText(int wave)
        {
            _waveText.text = $"Wave {wave} Incoming! Get Ready!";
            _waveTextObject.SetActive(true);
            _waveTextTimer.Start(_waveTextTime, () => {
                _waveTextObject.SetActive(false);
            });
        }
    }
}
