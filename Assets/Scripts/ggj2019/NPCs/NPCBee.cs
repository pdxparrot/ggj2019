using pdxpartyparrot.Core;
using UnityEngine;

public class NPCBee : NPCBase, ISwarmable
{

    [SerializeField] private float _speed = 5f;

    public bool InPlayerSwarm => _inplayerSwarm;

    private Vector3 _target = new Vector3(0,0,0);
    private bool _inplayerSwarm = false;

    #region Unity Life Cycle

    private void Start()
    {
        _target = transform.position;
    }

    private void Update()
    {
        if(PartyParrotManager.Instance.IsPaused)
            return;

        MoveToTarget();
    }

    #endregion

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target,
            _speed * Time.deltaTime);
    }

    public void SetTargetLocation(Vector3 location)
    {
        _target = location;
    }

    public void DoContext()
    {
        _inplayerSwarm = false;

        Defend();
    }

    public void PlayerSwarm()
    {
        _inplayerSwarm = true;
    }

#region the things they bee doing

    public void Swarm()
    {

    }

    public void Harvest()
    {

    }

    public void Defend()
    {
        _target = transform.position;
    }

    public void Repair()
    {

    }
#endregion
}
