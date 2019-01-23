#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
    public sealed class GameManager : Game.GameManager<GameManager>
    {
        public override bool IsGameOver => false;

#region Unity Lifecycle
        private void Awake()
        {
            GameStateManager.Instance.RegisterGameManager(this);
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterGameManager();
            }

            base.OnDestroy();
        }
#endregion

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);

            Debug.LogWarning("TODO: start game");
        }
    }
}
