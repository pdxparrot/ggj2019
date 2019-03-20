using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.NPCs
{
    public interface INPCBehavior
    {
        NPCBehaviorData NPCBehaviorData { get; }

        INPC NPC { get; }
    }
}
