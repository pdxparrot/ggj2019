using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : Game.State.GameOverState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            UI.PlayerUI playerUI = (UI.PlayerUI)UIManager.Instance.PlayerUI;
            playerUI.ShowPlayerHUD(false);
            playerUI.SetScoreText(GameManager.Instance.Score, GameManager.Instance.WaveSpawner.CurrentWaveIndex);
            playerUI.ShowGameOver(true);
        }
    }
}
