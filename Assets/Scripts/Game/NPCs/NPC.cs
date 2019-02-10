using pdxpartyparrot.Game.Data;

namespace pdxpartyparrot.Game.NPCs
{
    // TODO: this as an interface should be temporary
    // really what we want is a behavior that also requires an Actor
    // to marker something as an NPC and to give us hooks into NPC behaviors
    public interface INPC
    {
        void Initialize(NPCData data);
    }
}
