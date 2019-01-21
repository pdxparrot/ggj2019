#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Game;

using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
    public sealed class GameManager : Game.GameManager<GameManager>
    {
        public bool IsGameOver => false;

        //[Server]
        public void StartGame()
        {
            Assert.IsTrue(NetworkServer.active);
        }
    }
}
