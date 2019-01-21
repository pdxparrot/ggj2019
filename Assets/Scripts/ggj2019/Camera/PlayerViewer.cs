using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Camera;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Camera
{
    public sealed class PlayerViewer : FollowViewer, IPlayerViewer
    {
        [SerializeField]
        [ReadOnly]
        private Player _target;

        public Viewer Viewer => this;

        public void Initialize(Game.Players.Player target, int id)
        {
            Initialize(id);

            Set3D();

            _target = (Player)target;

            FollowCamera.SetTarget(_target.FollowTarget);
            SetFocus(_target.transform);
        }
    }
}
