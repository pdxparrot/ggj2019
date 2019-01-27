using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.ggj2019.UI;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : Game.State.GameOverState
    {
        public override void Initialize()
        {
            foreach(Players.Player player in PlayerManager.Instance.Actors) {
                HighScoreManager.Instance.AddHighScore($"{player.Id}", (int)GameManager.Instance.Score);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if(null != UIManager.Instance.PlayerUI) {
                ((UI.PlayerUI)(UIManager.Instance.PlayerUI)).ShowGameOver(true);
            }
        }
    }
}
