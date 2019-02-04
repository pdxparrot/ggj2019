using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.ggj2019.Players;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : Game.State.GameOverState
    {
        public override void Initialize()
        {
            foreach(Players.Player player in PlayerManager.Instance.Players) {
                HighScoreManager.Instance.AddHighScore($"{player.Id}", GameManager.Instance.Score);
            }
        }

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
