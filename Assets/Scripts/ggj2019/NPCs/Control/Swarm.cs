using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    [SerializeField] private float _swarmRadius = 0f;
    [SerializeField] private bool isPlayerSwarm = false;
    private List<ISwarmable> _iSwarmables = new List<ISwarmable>();
    private List<Vector3> _swarmOffset = new List<Vector3>();

    public void Add(ISwarmable iSwarmable)
    {
        _iSwarmables.Add(iSwarmable);

        if(isPlayerSwarm && iSwarmable.CanJoinSwarm)
            iSwarmable.JoinSwarm(this, _swarmRadius);
    }

    public bool DoContext()
    {
        if (!HasSwarm())
            return false;

        if(_iSwarmables[0].DoContext()) {
            _iSwarmables.RemoveAt(0);
            return true;
        }
        return false;
    }

    public bool HasSwarm()
    {
        return (_iSwarmables.Count > 0);
    }


    public int Kill(int amount)
    {
        int killed = 0;
        for(int i=0; i<_iSwarmables.Count && killed < amount; ++i) {
            _iSwarmables[i].Kill();
            killed++;
        }
        _iSwarmables.RemoveRange(0, killed);

        return killed;
    }
}
