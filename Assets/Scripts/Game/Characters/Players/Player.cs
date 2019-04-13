using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.Players
{
    public interface IPlayer
    {
        GameObject GameObject { get; }

        Guid Id { get; }

        bool IsLocalActor { get; }

        Game.Players.NetworkPlayer NetworkPlayer { get; }

        ActorBehavior Behavior { get; }

        IPlayerBehavior PlayerBehavior { get; }

        Viewer Viewer { get; }
    }
}
