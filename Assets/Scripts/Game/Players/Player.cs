﻿using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayer
    {
        GameObject GameObject { get; }

        Guid Id { get; }

        bool IsLocalActor { get; }

        NetworkPlayer NetworkPlayer { get; }

        ActorBehavior Behavior { get; }

        IPlayerController PlayerBehavior { get; }

        Viewer Viewer { get; }

        void Initialize(Guid id);
    }
}
