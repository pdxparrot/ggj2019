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

#region Animation
        [SerializeField]
        private GameObject _gameOverAnimationObject;

        [SerializeField]
        private SpineAnimationHelper _gameOverAnimationHelper;

        [SerializeField]
        private string _gameOverEntranceAnimation = "game_over_entrance";

        [SerializeField]
        private string _gameOverAnimation = "game_over_entrance2";
#endregion

#endregion

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
            _deathTextObject.SetActive(show);
        }

        public void ShowGameOver(bool show)
        {
            _gameOverAnimationObject.SetActive(show);
            _gameOverTextObject.SetActive(show);

            if(show) {
                TrackEntry track = _gameOverAnimationHelper.SetAnimation(_gameOverEntranceAnimation, false);
                track.Complete += t => {
                    _gameOverAnimationHelper.SetAnimation(_gameOverAnimation, true);
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
