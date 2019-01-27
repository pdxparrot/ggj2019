﻿using pdxpartyparrot.Core;
using pdxpartyparrot.ggj2019;
using UnityEngine;

using pdxpartyparrot.ggj2019.Players;
using Spine.Unity;

public class NPCWasp : NPCEnemy
{
    [SerializeField] private float MaxVel;
    [SerializeField] private float Accel;
    [SerializeField] private float pushback;
    [SerializeField] private float pushbackvel;
   
    private Vector3 _accel;
    private Vector3 _velocity;

    private void Start() {
        Pool.Add(this);

        float dir = (transform.position.x > 0) ? -1 : 1;
        _accel = new Vector3(1,0,0) * Accel * dir;
        _animation.Skeleton.ScaleX = _accel.x < 0 ? 1.0f : -1.0f;

        SetHoverAnimation();
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }

    void Update() {
        if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
            return;
        }

        _velocity += _accel * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);
        transform.position += _velocity * Time.deltaTime;

        SetFlightAnimation();
        _animation.Skeleton.ScaleX = _velocity.x < 0 ? 1.0f : -1.0f;

        var hive = Hive.Nearest(transform.position);
        if(hive.Collides(this)) {
            if(hive.TakeDamage(transform.position))
                Kill();
            else
                PushBack();
        }
    }



    // start true to force the animation the first time
    private bool _isFlying = true;

    private void SetHoverAnimation()
    {
        if(!_isFlying) {
            return;
        }

        SetAnimation("wasp_hover", true);
        _isFlying = false;
    }

    private void SetFlightAnimation()
    {
        if(_isFlying) {
            return;
        }

        //SetAnimation("wasp_attack", true);
        SetAnimation("wasp_hover", true);
        _isFlying = true;
    }

    void PushBack() {
        transform.position -= _velocity.normalized * pushback;
        _velocity = _velocity.normalized * pushbackvel;
    }

    public static ProxPool<NPCWasp> Pool = new ProxPool<NPCWasp>();

    public static NPCWasp Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCWasp;
    }
}
