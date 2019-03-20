using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class PlayerManager : Game.Players.PlayerManager<PlayerManager, Player>
    {
#region Debug
        [SerializeField]
        private bool _playersImmune;

        public bool PlayersImmune => _playersImmune;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            GameStateManager.Instance.RegisterPlayerManager(this);

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            if(GameStateManager.HasInstance) {
                GameStateManager.Instance.UnregisterPlayerManager();
            }

            base.OnDestroy();
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ggj2019.PlayerManager");
            debugMenuNode.RenderContentsAction = () => {
                _playersImmune = GUILayout.Toggle(_playersImmune, "Players Immune");
            };
        }
    }
}
