using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Home;

using Spine;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    // NOTE: wasp art faces the wrong direction so all facing stuff is transposed 
    public sealed class NPCWasp : NPCEnemy
    {
        // TODO: data
        [SerializeField] private float MaxVel;
        [SerializeField] private float Accel;

        [SerializeField]
        [ReadOnly]
        private Vector3 _acceleration;

        [SerializeField]
        [ReadOnly]
        private Vector3 _velocity;

#region Spline
        [SerializeField]
        [ReadOnly]
        private float _splineLen;

        [SerializeField]
        [ReadOnly]
        private float _splineVel;

        [SerializeField]
        [ReadOnly]
        private float _splinePos;

        [SerializeField]
        [ReadOnly]
        private BezierSpline _spline;
#endregion

        [SerializeField]
        private EffectTrigger _attackEffect;

        // TODO: make this a state enum
        private bool _isFlying = true;
        private bool _isAttacking;

        private TrackEntry _attackAnimation;

#region Unity Lifecycle
        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            Think(dt);
        }
#endregion

        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            Vector3 dir = (Vector3.zero - transform.position).normalized;
            _acceleration = dir * Accel;
            _spineAnimationHelper.SetFacing(-dir);

            SetHoverAnimation();

            _splineLen = 0.0f;
            _splinePos = 0.0f;
            _splineVel = 0.0f;

            var spline = spawnpoint.GetComponent<BezierSpline>();
            if(spline != null) {
                _spline = spline;
                _splineLen = spline.EstLength();
            }

            return true;
        }

        public override void OnDeSpawn()
        {
            if(null != _attackAnimation) {
                _attackAnimation.Complete -= OnAttackComplete;
                _attackAnimation = null;
            }

            _attackEffect.StopTrigger();

            _isAttacking = false;
            _isFlying = true;
            _spline = null;

            _acceleration = Vector3.zero;
            _velocity = Vector3.zero;

            base.OnDeSpawn();
        }

        public override void Initialize(NPCData data)
        {
            base.Initialize(data);
        }

        private void Think(float dt)
        {
            if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            if(_isAttacking) {
                return;
            }

	        if(!FollowSpline(dt)) {
	            _velocity += _acceleration * dt;
	            _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);

	            transform.position += _velocity * dt;

                _spineAnimationHelper.SetFacing(-_velocity);
	        }

            SetFlightAnimation();

            if(Hive.Instance.Collides(this)) {
                Attack(Hive.Instance);
            }
        }

        private bool FollowSpline(float dt)
        {
            if(null == _spline) {
                return false;
            }

	        _splineVel += _acceleration.magnitude * dt;
	        _splineVel = Mathf.Max(_splineVel, MaxVel);
	        _splinePos += _splineVel * dt;

	        float t = _splinePos / _splineLen;

	        Vector3 targetLocation = _spline.GetPoint(t);

            _spineAnimationHelper.SetFacing(transform.position - targetLocation);

            transform.position = targetLocation;

            return true;
        }

        private void SetHoverAnimation()
        {
            if(!_isFlying) {
                return;
            }

            _spineAnimationHelper.SetAnimation("wasp_hover", true);
            _isFlying = false;
        }

        private void SetFlightAnimation()
        {
            if(_isFlying) {
                return;
            }

            _spineAnimationHelper.SetAnimation("wasp_hover", true);
            _isFlying = true;
        }

        private void SetAttackAnimation()
        {
            if(null != _attackAnimation) {
                _attackAnimation.Complete -= OnAttackComplete;
                _attackAnimation = null;
            }

            _isAttacking = true;

            _attackAnimation = _spineAnimationHelper.SetAnimation(1, "wasp_attack", false);
            _attackAnimation.Complete += OnAttackComplete;
        }

        private void OnAttackComplete(TrackEntry track)
        {
            _isAttacking = false;

            if(IsDead) {
                return;
            }

            if(Hive.Instance.Damage(transform.position)) {
                Kill(false);
            }
        }

        private void Attack(Hive hive)
        {
            if(_isAttacking) {
                return;
            }

            _velocity = Vector3.zero;

            SetAttackAnimation();
            _attackEffect.Trigger();
        }

        public override void Kill(bool playerKill)
        {
            if(playerKill) {
                GameManager.Instance.WaspKilled();
            }

            base.Kill(playerKill);
        }
    }
}
