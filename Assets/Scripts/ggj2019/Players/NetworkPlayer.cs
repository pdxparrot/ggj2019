#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Game.Players;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(NetworkAnimator))]
    public sealed class NetworkPlayer : Game.Players.NetworkPlayer
    {
    }
}
