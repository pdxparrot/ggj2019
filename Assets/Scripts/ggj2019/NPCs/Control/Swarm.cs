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

        _iSwarmables[0].DoContext();
       _iSwarmables.RemoveAt(0);
       return true;
    }

    public bool HasSwarm()
    {
        return (_iSwarmables.Count > 0);
    }

    public int kill(int amount)
    {
        if (amount >= _iSwarmables.Count)
        {

            int len = _iSwarmables.Count;

            for (int i = 0; i < _iSwarmables.Count; i++)
            {
                _iSwarmables[i].Kill();
            }
            _iSwarmables.Clear();

            return amount - len;
        }

        for (int i = 0; i < amount; i++)
        {
            _iSwarmables[i].Kill();
        }

        _iSwarmables.RemoveRange(0,amount);

        return 0;
    }
}
