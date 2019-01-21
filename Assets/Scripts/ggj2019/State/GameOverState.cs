using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : SubGameState
    {
        [SerializeField]
        private float _completeWaitTimeSeconds = 5.0f;

        [SerializeField]
        [ReadOnly]
        private Timer _completeTimer;

        public void Initialize()
        {
            foreach(Player player in PlayerManager.Instance.Actors) {
                Debug.Log("TODO: Add High Score");
                HighScoreManager.Instance.AddHighScore($"{player.Id}", 0);
            }
        }

        public override void OnEnter()
        {
Debug.Log("TODO: em over Player HUD");
            /*if(null != UIManager.Instance.PlayerUI) {
                UIManager.Instance.PlayerUI.PlayerHUD.ShowGameOverText();
            }*/

            _completeTimer.Start(_completeWaitTimeSeconds, () => {
                GameStateManager.Instance.TransitionToInitialState();
            });
        }

        public override void OnUpdate(float dt)
        {
            _completeTimer.Update(dt);
        }
    }
}
