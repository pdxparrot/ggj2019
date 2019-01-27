#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.State;

using pdxpartyparrot.ggj2019.Camera;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019
{
    public sealed class GameManager : Game.GameManager<GameManager>
    {
        public GameData GameGameData => (GameData)GameData;

        public GameViewer Viewer { get; private set; }

        public override bool IsGameOver { get; protected set; }

        [SerializeField]
        [ReadOnly]
        private int _round = 1;

        public int Round => _round;

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

            SpawnManager.Instance.Initialize();

            IsGameOver = false;
        }

        //[Client]
        public void InitViewer()
        {
            Viewer = ViewerManager.Instance.AcquireViewer<GameViewer>();
            if(null != Viewer) {
                Viewer.Set2D();
                Viewer.Camera.orthographicSize = GameGameData.GameSize2D;
                Viewer.transform.position = GameGameData.ViewerPosition;
            }
        }
    }
}
