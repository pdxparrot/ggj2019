using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Game.State;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class MainGameState : Game.State.MainGameState
    {
        protected override bool InitializeClient()
        {
            if(!base.InitializeClient()) {
                return false;
            }

            //ViewerManager.Instance.AllocateViewers(1, PlayerManager.Instance.PlayerData.PlayerViewerPrefab);

            return true;
        }
    }
}
