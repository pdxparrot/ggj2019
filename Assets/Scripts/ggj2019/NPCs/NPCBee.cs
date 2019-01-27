using pdxpartyparrot.Core;
using UnityEngine;
using pdxpartyparrot.Core.Util;

public class NPCBee : NPCBase, ISwarmable
{

    [SerializeField] private float _speed = 5f;
    private Vector2 _offsetChangeTimer = new Vector2(0.2f,0.5f);

    public bool InPlayerSwarm => _inplayerSwarm;

    private Vector3 _target = new Vector3(0,0,0);
    private bool _inplayerSwarm = false;

    private float _offsetRadius = 0f;
    private Vector3 _offsetPosition = new Vector3(0,0,0);
    private Timer _offsetUpdateTimer;
    #region Unity Life Cycle

    private void Start()
    {
        _target = transform.position;

        _offsetUpdateTimer = new Timer();
        _offsetUpdateTimer.Start(
            PartyParrotManager.Instance.Random.NextSingle(
                _offsetChangeTimer.x,
                _offsetChangeTimer.y)
            );
    }

    private void Update()
    {
        if(PartyParrotManager.Instance.IsPaused)
            return;

        CheckTimers();

        MoveToTarget();


    }

    private void CheckTimers()
    {
        _offsetUpdateTimer.Update(Time.deltaTime);

        if (!_offsetUpdateTimer.IsRunning)
        {
            UpdateOffset();

            _offsetUpdateTimer.Start(
                PartyParrotManager.Instance.Random.NextSingle(
                    _offsetChangeTimer.x,
                    _offsetChangeTimer.y)
            );
        }
    }

    #endregion

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target+_offsetPosition,
            _speed * Time.deltaTime);
    }

    public void SetTargetLocation(Vector3 location)
    {
        _target = location;
    }

    public void DoContext()
    {
        _inplayerSwarm = false;


        _offsetRadius = 0;
        UpdateOffset();

        Defend();
    }

    private void UpdateOffset()
    {
        _offsetPosition = new Vector3(
            Random.Range(-_offsetRadius, _offsetRadius),
            Random.Range(-_offsetRadius, _offsetRadius),
            Random.Range(-_offsetRadius, _offsetRadius)
            );
    }

    public void SetSwarmRadius(float radius)
    {
        _offsetRadius = radius;
        UpdateOffset();
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
