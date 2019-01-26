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
            foreach(Players.Player player in PlayerManager.Instance.Actors) {
                HighScoreManager.Instance.AddHighScore($"{player.Id}", GameManager.Instance.Round);
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
