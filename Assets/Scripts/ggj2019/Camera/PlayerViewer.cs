using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Camera
{
    public sealed class PlayerViewer : FollowViewer3D, IPlayerViewer
    {
        [SerializeField]
        [ReadOnly]
        private Player _player;

        public Viewer Viewer => this;

        public void Initialize(IPlayer player, int id)
        {
            Initialize(id);

            _player = (Player)player;

            FollowCamera3D.SetTarget(_player.FollowTarget);
            SetFocus(_player.transform);
        }
    }
}
