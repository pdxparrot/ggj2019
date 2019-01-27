﻿using JetBrains.Annotations;
using pdxpartyparrot.Core;
using UnityEngine;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019.Players;

public class NPCBee : NPCBase, ISwarmable
{

    private enum NPCBeeState
    {
        Defend,
        Follow,
        Harvest,
        ReturnHome,
        Repair
    }

    [SerializeField] private float _speed = 5f;
    private Vector2 _offsetChangeTimer = new Vector2(0.2f,0.5f);

    [SerializeField] private float _attackDistance = 5.0f;
    [SerializeField] private float _repairCooldown = 10.0f;
    [SerializeField] private float _harvestTime = 10.0f;

    private readonly Timer _repairCooldownTimer = new Timer();
    private readonly Timer _harvestCooldownTimer = new Timer();

    private float _offsetRadius;
    private Vector2 _offsetPosition = new Vector3(0, 0);
    private readonly Timer _offsetUpdateTimer = new Timer();

    [SerializeField]
    [ReadOnly]
    private NPCBeeState _state = NPCBeeState.Defend;

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

    #region Unity Life Cycle

    private void Start()
    {
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

    #endregion

    private void Think(float dt)
    {
        switch(_state)
        {
        case NPCBeeState.Defend:
            Defend(dt);
            break;
        case NPCBeeState.Follow:
            Swarm(dt);
            break;
        case NPCBeeState.Harvest:
            Harvest(dt);
            break;
        case NPCBeeState.ReturnHome:
            ReturnHome(dt);
            break;
        case NPCBeeState.Repair:
            Repair(dt);
            break;
        }
    }

    public void DoContext()
    {
        _targetSwarm = null;
        _targetHive = null;
        _targetFlower = null;

        Hive hive = Hive.Nearest(transform.position);
        //NPCFlower flower = NPCFlower.Nearest(transform.position);

        if(hive != null && hive.Collides(transform.position)) {
            SetState(NPCBeeState.Repair);
            _targetHive = hive;
            _repairCooldownTimer.Start(_repairCooldown);
        /*} else if(flower != null && flower.Collides(transform.position)) {
            SetState(NPCBeeState.Harvest);
            _targetFlower = flower;
            _harvestTimer.Start(_harvestTime)*/
        } else {
            SetState(NPCBeeState.Defend);
        }
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

    private void AcquireEnemy()
    {
// TODO
    }

#region the things they bee doing

    private void Swarm(float dt)
    {
        if(null == _targetSwarm) {
            // ok?
            SetState(NPCBeeState.Defend);
            return;
        }

        MoveToTarget(dt, _targetSwarm.transform);
    }

    private void Harvest(float dt)
    {
        if(null == _targetFlower) {
            // someone took it?
            _harvestCooldownTimer.Stop();
            SetState(NPCBeeState.Defend);
            return;
        }

        if(_harvestCooldownTimer.IsRunning) {
            return;
        }

        _pollenCount = _targetFlower.Harvest();
        _targetFlower = null;

        SetState(NPCBeeState.ReturnHome);
    }

    private void ReturnHome(float dt)
    {
        if(null == _targetHive) {
            // waaat?
            SetState(NPCBeeState.Defend);
            return;
        }

        if(_targetHive.Collides(transform.position)) {
            _targetHive.UnloadPollen(_pollenCount);
            _pollenCount = 0;

            _targetHive = null;

            SetState(NPCBeeState.Defend);
            return;
        }

        MoveToTarget(dt, _targetHive.transform);
    }

    private void Defend(float dt)
    {
        if(null == _targetEnemy) {
            AcquireEnemy();
            if(null== _targetEnemy) {
                return;
            }
        }

        MoveToTarget(dt, _targetEnemy.transform);
// TODO: this is going to crash when we kill it
    }

    private void Repair(float dt)
    {
        if(null == _targetHive) {
            // whoops...
            SetState(NPCBeeState.Defend);
            return;
        }

        if(_repairCooldownTimer.IsRunning) {
            return;
        }

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

        transform.position = Vector2.MoveTowards(transform.position,position, _speed * dt);
    }
#endregion
}
