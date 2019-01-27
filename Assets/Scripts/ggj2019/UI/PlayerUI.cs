using UnityEngine;

namespace pdxpartyparrot.ggj2019.UI
{
    public sealed class PlayerUI : Game.UI.PlayerUI
    {
        [SerializeField]
        private GameObject _gameOverText;

        public override void Initialize(UnityEngine.Camera uiCamera)
        {
            base.Initialize(uiCamera);

            ShowGameOver(false);
        }

        public void ShowGameOver(bool show)
        {
            _gameOverText.SetActive(show);
        }
    }
}
