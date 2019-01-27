using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Swarm : MonoBehaviour
{

    [SerializeField] private bool isPlayerSwarm = false;
    private List<ISwarmable> _iSwarmables = new List<ISwarmable>();


    #region Unity Life Cycle

    private void Update()
    {
        UpdateTargetLocations();
    }

    #endregion

    private void UpdateTargetLocations()
    {
        int len = _iSwarmables.Count;

        for (int i = 0; i < len; i++)
        {
            _iSwarmables[i].SetTargetLocation(transform.position);
        }
    }

    public void Add(ISwarmable iSwarmable)
    {
        _iSwarmables.Add(iSwarmable);
        if(isPlayerSwarm)
            iSwarmable.PlayerSwarm();
    }

    public bool DoContext()
    {
        if (!HasSwarm())
            return false;

        // TODOsomthing other then defend
        _iSwarmables[0].DoContext();
       _iSwarmables.RemoveAt(0);
       return true;
    }

    public bool HasSwarm()
    {
        return (_iSwarmables.Count > 0);
    }
}
