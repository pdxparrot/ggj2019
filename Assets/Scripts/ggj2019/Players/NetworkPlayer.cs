#pragma warning disable 0618    // disable obsolete warning for now

using System;

using pdxpartyparrot.Game.Players;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(NetworkAnimator))]
    public sealed class NetworkPlayer : Game.Players.NetworkPlayer
    {
#region Callbacks
        [ClientRpc]
        public override void RpcSpawn(string id)
        {
            if(null != Player) {
                Player.Initialize(new Guid(id));
            }
        }
#endregion
    }
}
