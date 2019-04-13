using System;

using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.NPCs
{
    public interface INPC
    {
        GameObject GameObject { get; }

        Guid Id { get; }

        ActorBehavior Behavior { get; }

        INPCBehavior NPCBehavior { get; }

        void Recycle();
    }
}
