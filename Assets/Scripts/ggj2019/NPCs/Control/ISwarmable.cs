namespace pdxpartyparrot.ggj2019.NPCs.Control
{
    // TODO: make this core
    public interface ISwarmable
    {
        bool CanJoinSwarm { get; }

        bool IsInSwarm { get; }

        void JoinSwarm(Swarm swarm, float radius);

        void Kill();

    }
}
