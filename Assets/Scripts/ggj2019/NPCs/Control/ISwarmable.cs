using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwarmable
{
    void PlayerSwarm();
    void SetTargetLocation(Vector3 location);
    void DoContext();

}
