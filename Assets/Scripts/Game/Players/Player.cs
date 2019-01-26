﻿using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;

using UnityEngine;

namespace pdxpartyparrot.Game.Players
{
    public interface IPlayer
    {
        string Name { get; }

        bool IsLocalActor { get; }

        NetworkPlayer NetworkPlayer { get; }

        ActorController Controller { get; }

        Viewer Viewer { get; }

        Vector3 Position { get; }

        void Initialize(int id);
    }
}