using UnityEngine;

public interface ISwarmable
{
    bool CanJoinSwarm { get; }

    bool IsInSwarm { get; }

    void JoinSwarm(Swarm swarm, float radius);

    bool DoContext();

    void Kill();

}
