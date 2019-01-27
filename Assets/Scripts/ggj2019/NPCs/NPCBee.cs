using JetBrains.Annotations;
using pdxpartyparrot.Core;
using UnityEngine;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game;
using pdxpartyparrot.Game.Effects;
using UnityEngine.Assertions;

public class NPCBee : NPCBase, ISwarmable
{

    private enum NPCBeeState
    {
        Idle,
        Follow,
        PathToHarvest,
        Harvest,
        ReturnHome,
        PathToRepair,
        Repair
    }

    [SerializeField] private float _swarmSpeedModifier = 2.0f;
    [SerializeField] private float _fullSPeedModifier = 0.5f;

    [SerializeField]
    [ReadOnly]
    private Vector2 _offsetChangeTimer = new Vector2(0.2f,0.5f);

    [SerializeField] private float _harvestDistance = 1.0f;
    [SerializeField] private float _harvestTime = 10.0f;
    [SerializeField] private float _repairDistance = 1.0f;
    [SerializeField] private float _repairCooldown = 10.0f;

    private readonly Timer _repairCooldownTimer = new Timer();
    private readonly Timer _harvestCooldownTimer = new Timer();

    private float _offsetRadius;
    private Vector2 _offsetPosition = new Vector3(0, 0);
    private readonly Timer _offsetUpdateTimer = new Timer();

    [SerializeField]
    [ReadOnly]
    private NPCBeeState _state = NPCBeeState.Idle;

    [SerializeField]
    [ReadOnly]
    [CanBeNull]
    private Swarm _targetSwarm;

    public bool IsInSwarm => null != _targetSwarm;

    public bool CanJoinSwarm => !IsInSwarm && _state != NPCBeeState.ReturnHome;

    [SerializeField]
    [ReadOnly]
    [CanBeNull]
    private Hive _targetHive;

    [SerializeField]
    [ReadOnly]
    [CanBeNull]
    private NPCEnemy _targetEnemy;

    [SerializeField]
    [ReadOnly]
    [CanBeNull]
    private NPCFlower _targetFlower;

    [SerializeField]
    [ReadOnly]
    private int _pollenCount;

    [SerializeField]
    private EffectTrigger _deathEffect;

    #region Unity Life Cycle

    private void Start()
    {
        Pool.Add(this);

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

        float dt = Time.deltaTime;

        CheckTimers(dt);
        Think(dt);
    }

    protected override void OnDestroy()
    {
        Pool.Remove(this);

        base.OnDestroy();
    }

    #endregion

    private void CheckTimers(float dt)
    {
        _offsetUpdateTimer.Update(dt);

        if (!_offsetUpdateTimer.IsRunning)
        {
            UpdateOffset();

            _offsetUpdateTimer.Start(
                PartyParrotManager.Instance.Random.NextSingle(
                    _offsetChangeTimer.x,
                    _offsetChangeTimer.y)
            );
        }

        _repairCooldownTimer.Update(dt);
        _harvestCooldownTimer.Update(dt);
    }


    private void Think(float dt)
    {
        // TODO: but still let them flock, that's cool looking
        if(GameManager.Instance.IsGameOver || PartyParrotManager.Instance.IsPaused) {
            return;
        }

        switch(_state)
        {
        case NPCBeeState.Idle:
            break;
        case NPCBeeState.Follow:
            Swarm(dt);
            break;
        case NPCBeeState.PathToHarvest:
            PathToHarvest(dt);
            break;
        case NPCBeeState.Harvest:
            Harvest(dt);
            break;
        case NPCBeeState.ReturnHome:
            ReturnHome(dt);
            break;
        case NPCBeeState.PathToRepair:
            PathToRepair(dt);
            break;
        case NPCBeeState.Repair:
            Repair(dt);
            break;
        }
    }

    public bool DoContext()
    {
        Vector2 position = transform.position;

        Hive hive = Hive.Nearest(position);
        NPCFlower flower = NPCFlower.Nearest(position);

        if(hive != null && hive.Collides(Collider.bounds, _repairDistance)) {
// TODO: leave our swarm?
            _targetSwarm = null;
            _targetHive = hive;

            SetState(NPCBeeState.PathToRepair);
            return true;
        } else if(flower != null && flower.Collides(Collider.bounds, _harvestDistance)) {
// TODO: leave our swarm?
            _targetSwarm = null;
            _targetFlower = flower;

            SetState(NPCBeeState.PathToHarvest);
            return true;
        /*} else {
            SetState(NPCBeeState.Idle);*/
        }

        return false;
    }

    public void Kill()
    {
        _deathEffect.Trigger(() => {
            Destroy(gameObject);
        });
    }

    public void JoinSwarm(Swarm swarm, float radius)
    {
        if(!CanJoinSwarm) {
            return;
        }

        _targetSwarm = swarm;
        SetState(NPCBeeState.Follow);

        _offsetRadius = radius;
        UpdateOffset();
    }

    private void UpdateOffset()
    {
        _offsetPosition = new Vector2(
            Random.Range(-_offsetRadius, _offsetRadius),
            Random.Range(-_offsetRadius, _offsetRadius)
        );
    }

    private void SetState(NPCBeeState state)
    {
        Debug.Log($"setting state: {state}");
        _state = state;
    }

    private float CurrentSpeed()
    {
        float modifier = 1.0f;
        if(IsInSwarm) {
            modifier = _swarmSpeedModifier;
        } else if(_pollenCount > 0) {
            modifier = _fullSPeedModifier;
        }

        return PlayerManager.Instance.PlayerData.PlayerControllerData.MoveSpeed * modifier;
    }

    private bool AcquireFlower()
    {
        _targetFlower = NPCFlower.Nearest(transform.position);
        return null != _targetFlower && _targetFlower.Collides(Collider.bounds, _harvestDistance);
    }

    private bool AcquireHive()
    {
        _targetHive = Hive.Nearest(transform.position);
        return null != _targetHive;
    }

#region the things they bee doing

    private void Swarm(float dt)
    {
        if(null == _targetSwarm) {
            Debug.LogWarning("lost my swarm!");
            SetState(NPCBeeState.ReturnHome);
            return;
        }

        MoveToTarget(dt, _targetSwarm.transform);
    }

    private void PathToHarvest(float dt)
    {
        if(null == _targetFlower && !AcquireFlower()) {
            Debug.LogError("target flower disappeared");
            SetState(NPCBeeState.ReturnHome);
            return;
        }

        if(_targetFlower.Collides(Collider.bounds)) {
            _harvestCooldownTimer.Start(_harvestTime);
            SetState(NPCBeeState.Harvest);
            return;
        }

        MoveToTarget(dt, _targetFlower.transform);
    }

    private void Harvest(float dt)
    {
        if(null == _targetFlower) {
            if(AcquireFlower()) {
                SetState(NPCBeeState.PathToHarvest);
                return;
            }

            Debug.LogError("target flower disappeared");
            SetState(NPCBeeState.ReturnHome);
            return;
        }

        if(_harvestCooldownTimer.IsRunning) {
            return;
        }

        _pollenCount = _targetFlower.Harvest();
        Assert.IsTrue(_pollenCount > 0);

        _targetFlower = null;

        Debug.Log($"harvested {_pollenCount} pollen");
        SetState(NPCBeeState.ReturnHome);
    }

    private void ReturnHome(float dt)
    {
        if(null == _targetHive && !AcquireHive()) {
            Debug.LogError("No hive to return to!");
            SetState(NPCBeeState.Idle);
            return;
        }

        if(_targetHive.Collides(Collider.bounds)) {
            _targetHive.UnloadPollen(null, _pollenCount);
            _pollenCount = 0;

            _targetHive = null;

            SetState(NPCBeeState.Idle);
            return;
        }

        MoveToTarget(dt, _targetHive.transform);
    }

    private void PathToRepair(float dt)
    {
        if(null == _targetHive && !AcquireHive()) {
            Debug.LogError("No hive to repair!");
            SetState(NPCBeeState.Idle);
            return;
        }

        if(_targetHive.Collides(Collider.bounds)) {
            _repairCooldownTimer.Start(_repairCooldown);
            SetState(NPCBeeState.Repair);
            return;
        }

        MoveToTarget(dt, _targetHive.transform);
    }

    private void Repair(float dt)
    {
        if(null == _targetHive) {
            if(AcquireHive()) {
                SetState(NPCBeeState.PathToRepair);
                return;
            }

            Debug.LogError("No hive to repair!");
            SetState(NPCBeeState.Idle);
            return;
        }

        if(_repairCooldownTimer.IsRunning) {
            return;
        }

        Debug.Log("Repairing...");
        _targetHive.Repair();
        _repairCooldownTimer.Start(_repairCooldown);
    }

    private void MoveToTarget(float dt, Transform target)
    {
        if(null == target) {
            return;
        }

        Vector2 position = target.position;
        if(IsInSwarm) {
            position += _offsetPosition;
        }

        transform.position = Vector2.MoveTowards(transform.position,position, CurrentSpeed() * dt);
    }
#endregion

    public static ProxPool<NPCBee> Pool = new ProxPool<NPCBee>();
}
