using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Splines;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Home;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCWasp : NPCEnemy
    {
        private enum State
        {
            Idle,
            FollowingSpline,
            Attacking,
        }

#region Spline
        [SerializeField]
        [ReadOnly]
        private BezierSpline _spline;

        [SerializeField]
        [ReadOnly]
        private float _splineLength;

        [SerializeField]
        [ReadOnly]
        private float _splinePosition;
#endregion

#region Effects
        [SerializeField]
        private EffectTrigger _attackStartEffect;

        [SerializeField]
        private EffectTrigger _attackEndEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        private NPCWaspData WaspData => (NPCWaspData)NPCData;

#region Unity Lifecycle
        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            Think(dt);
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _spineAnimationHelper.SetFacing(Vector3.zero - transform.position);

            _spline = spawnpoint.GetComponent<BezierSpline>();
            _splineLength = _spline.EstimatedLength();
            _splinePosition = 0.0f;

            return true;
        }

        public override void OnDeSpawn()
        {
            _attackEndEffect.StopTrigger();
            _attackStartEffect.StopTrigger();

            _spline = null;

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is NPCWaspData);

            base.Initialize(data);

            SetState(State.FollowingSpline);
        }

        private void SetState(State state)
        {
            _state = state;
            switch(state)
            {
            case State.Idle:
                SetIdleAnimation();
                break;
            case State.FollowingSpline:
                SetFlyingAnimation();
                break;
            case State.Attacking:
                Attack();
                break;
            }
        }

        private void Think(float dt)
        {
            if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            switch(_state)
            {
            case State.Idle:
                if(Hive.Instance.Collides(this)) {
                    SetState(State.Attacking);
                }
                break;
            case State.FollowingSpline:
                FollowSpline(dt);
                break;
            case State.Attacking:
                break;
            }
        }

#region Animation
        private void SetIdleAnimation()
        {
            _spineAnimationHelper.SetAnimation(WaspData.IdleAnimationName, true);
        }

        private void SetFlyingAnimation()
        {
            _spineAnimationHelper.SetAnimation(WaspData.FlyingAnimationName, true);
        }
#endregion

        public override void Kill(bool playerKill)
        {
            if(playerKill) {
                GameManager.Instance.WaspKilled();
            }

            base.Kill(playerKill);
        }

#region Actions
        private void Attack()
        {
            _attackStartEffect.Trigger(DoAttack);
        }

        private void DoAttack()
        {
            _attackEndEffect.Trigger();

            if(Hive.Instance.Damage(transform.position)) {
                Kill(false);
            } else {
                SetState(State.Idle);
            }
        }

        private void FollowSpline(float dt)
        {
            float speed = WaspData.Speed;

	        _splinePosition += speed * dt;
	        float t = _splinePosition / _splineLength;
	        Vector3 targetPosition = _spline.GetPoint(t);

            _spineAnimationHelper.SetFacing(targetPosition - transform.position);
            //transform.LookAt2DFlip(targetPosition);

            transform.position = targetPosition;

            if(_splinePosition >= _splineLength) {
                SetState(State.Idle);
            }
        }
#endregion
    }
}
