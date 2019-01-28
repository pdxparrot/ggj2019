using System;
using pdxpartyparrot.Core;
using pdxpartyparrot.ggj2019;
using UnityEngine;

using pdxpartyparrot.ggj2019.Players;
using Spine;
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

        if(!_isAttacking) {
            _velocity += _accel * Time.deltaTime;
            _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);
            transform.position += _velocity * Time.deltaTime;

            SetFlightAnimation();
            _animation.Skeleton.ScaleX = _velocity.x < 0 ? 1.0f : -1.0f;

            var hive = Hive.Nearest(transform.position);
            if(hive.Collides(this)) {
                Attack(hive);
            }
        }
    }


    // start true to force the animation the first time
    private bool _isFlying = true;
    private bool _isAttacking = false;

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

        SetAnimation("wasp_hover", true);
        _isFlying = true;
    }

    private void SetAttackAnimation(Action callback=null)
    {
        _isAttacking = true;
        TrackEntry track = SetAnimation(1, "wasp_attack", false);
        track.Complete += x => {
            _isAttacking = false;
            callback?.Invoke();
        };
    }

    private void Attack(Hive hive) {
        if(_isAttacking) {
            return;
        }

        _velocity = Vector3.zero;
        SetAttackAnimation(() => {
            if(hive.TakeDamage(transform.position)) {
                Kill();
            } else {
                PushBack();
            }
        });
    }

    void PushBack() {
        Vector3 pos = transform.position;
        Vector3 dir = new Vector3(-pos.x, 0.0f, 0.0f).normalized;

        transform.position -= dir * pushback;
        _velocity = dir * pushbackvel;
    }

    public static ProxPool<NPCWasp> Pool = new ProxPool<NPCWasp>();

    public static NPCWasp Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCWasp;
    }
}
