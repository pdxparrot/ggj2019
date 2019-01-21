using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class MainGameState : Game.State.MainGameState
    {
        [SerializeField]
        private GameOverState _gameOverState;

        public override void OnUpdate(float dt)
        {
            if(GameManager.Instance.IsGameOver) {
                GameStateManager.Instance.PushSubState(_gameOverState, state => {
                    state.Initialize();
                });
            }
        }

        protected override bool InitializeServer()
        {
            if(!base.InitializeServer()) {
                return false;
            }

            GameManager.Instance.StartGame();

            return true;
        }

        protected override bool InitializeClient()
        {
            if(!base.InitializeClient()) {
                return false;
            }

            ViewerManager.Instance.AllocateViewers(1, PlayerManager.Instance.PlayerData.PlayerViewerPrefab as PlayerViewer);

            return true;
        }
    }
}
