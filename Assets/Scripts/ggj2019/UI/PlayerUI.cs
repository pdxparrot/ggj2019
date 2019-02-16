using pdxpartyparrot.Core.Util;

using Spine;
using Spine.Unity;
using TMPro;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerUI : Game.UI.PlayerUI
    {
        [SerializeField]
        private PlayerHUD _playerHUD;

        [SerializeField]
        private GameObject _introTextObject;

        [SerializeField]
        private float _introTextTime = 2.5f;

        [SerializeField]
        private GameObject _deathText;

        [SerializeField]
        private TextMeshProUGUI _endGameWaveText;

        [SerializeField]
        private TextMeshProUGUI _endGameScoreText;

        [SerializeField]
        private SkeletonAnimation _animatedGameOverObject;

        [SerializeField]
        private GameObject _gameOverText;

        [SerializeField]
        private TextMeshProUGUI _waveText;

        [SerializeField]
        private GameObject _waveTextObject;

        [SerializeField]
        private float _waveTextTime = 2.5f;

        private readonly Timer _waveTextTimer = new Timer();
        private readonly Timer _introTextTimer = new Timer();

#region Unity Lifecycle
        private void Update()
        {
            float dt = Time.deltaTime;

            _waveTextTimer.Update(dt);
            _introTextTimer.Update(dt);
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
            _deathText.SetActive(show);
        }

        public void ShowGameOver(bool show)
        {
            _animatedGameOverObject.ClearState();
            _animatedGameOverObject.gameObject.SetActive(show);
            _gameOverText.SetActive(show);

            if(show) {
                TrackEntry track = _animatedGameOverObject.AnimationState.SetAnimation(0, "game_over_entrance", false);
                track.Complete += t => {
                    _animatedGameOverObject.AnimationState.SetAnimation(0, "game_over_entrance2", true);
                };

                ShowDeathText(false);
                _waveTextObject.SetActive(false);
                _waveTextTimer.Stop();
            }
        }

        public void SetScoreText(int score, int wave)
        {
            _endGameWaveText.text = $"You Made It To Wave {wave}!";
            _endGameScoreText.text = $"Your Score: {score}";
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
