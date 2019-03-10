using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.NPCs
{
    public interface INPC
    {
        GameObject GameObject { get; }

        Guid Id { get; }

        ActorBehavior Behavior { get; }

        INPCBehavior NPCBehavior { get; }

        void Initialize(Guid id, NPCData data);
    }
}
