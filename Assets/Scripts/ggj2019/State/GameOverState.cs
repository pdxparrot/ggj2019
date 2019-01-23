using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : Game.State.GameOverState
    {
        public override void Initialize()
        {
            foreach(Player player in PlayerManager.Instance.Actors) {
                Debug.LogWarning("TODO: Add High Score");
                HighScoreManager.Instance.AddHighScore($"{player.Id}", 0);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

Debug.LogWarning("TODO: Game over Player HUD");
            /*if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowGameOverText();
            }*/
        }
    }
}
