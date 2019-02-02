using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.Effects;

using Spine;

using UnityEngine;

public class NPCWasp : NPCEnemy
{
    [SerializeField] private float MaxVel;
    [SerializeField] private float Accel;
    [SerializeField] private float pushback;
    [SerializeField] private float pushbackvel;
   
    private Vector3 _accel;
    private Vector3 _velocity;

    private float _splineLen;
    private float _splineVel;
    private float _splinePos;

    [SerializeField]
    [ReadOnly]
    private BezierSpline _spline;

    [SerializeField]
    private EffectTrigger _attackEffect;

#region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();

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

    private void Update()
    {
        float dt = Time.deltaTime;

        Think(dt);
    }
#endregion

    public override void OnSpawn(SpawnPoint spawnpoint)
    {
        base.OnSpawn(spawnpoint);

        var spline = spawnpoint.GetComponent<BezierSpline>();
        if(spline != null) {
            _spline = spline;
            _splineLen = spline.EstLength();
        }
    }

    private void Think(float dt)
    {
        if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
            return;
        }

        if(_isAttacking) {
            return;
        }

	    if(_spline) {
	        _splineVel += _accel.magnitude * dt;
	        _splineVel = Mathf.Max(_splineVel, MaxVel);
	        _splinePos += _splineVel * dt;

	        float t = _splinePos / _splineLen;

	        Vector3 targetLocation = _spline.GetPoint(t);

            SetFacingDirection(targetLocation.x - transform.position.x);

            transform.position = targetLocation;
        }
	    else {
	        _velocity += _accel * Time.deltaTime;
	        _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);

	        transform.position += _velocity * Time.deltaTime;

            SetFacingDirection(_velocity.x);
	    }

        SetFlightAnimation();

        var hive = Hive.Nearest(transform.position);
        if(hive.Collides(this)) {
            Attack(hive);
        }
    }

    private void SetFacingDirection(float xDirection)
    {
        if(xDirection < 0.02f && xDirection > -0.02f)
            return;

        _animation.Skeleton.ScaleX = xDirection < 0 ? 1.0f : -1.0f;
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
        _attackEffect.Trigger();
    }

    private void PushBack()
    {
        Vector3 pos = transform.position;
        Vector3 dir = new Vector3(-pos.x, 0.0f, 0.0f).normalized;

        transform.position -= dir * pushback;
        _velocity = dir * pushbackvel;
    }

    public override void Kill()
    {
        base.Kill();

        GameManager.Instance.WaspKilled();
    }

    public static ProxPool<NPCWasp> Pool = new ProxPool<NPCWasp>();

    public static NPCWasp Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist);
    }
}
