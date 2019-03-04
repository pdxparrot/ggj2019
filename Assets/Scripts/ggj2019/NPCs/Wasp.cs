using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Splines;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.World;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Wasp : Enemy
    {
        private enum State
        {
            Idle,
            FollowingSpline,
            Attacking,
            Dead
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

        [SerializeField]
        [ReadOnly]
        private float _splineYOffset;
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

        public override bool IsDead => _state == State.Dead;

        [SerializeField]
        [ReadOnly]
        private HiveArmor _armorToAttack;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _attackCooldownTimer = new Timer();

        private WaspData WaspData => (WaspData)NPCData;

#region Unity Lifecycle
        protected override void Update()
        {
            float dt = Time.deltaTime;

            _attackCooldownTimer.Update(dt);

            base.Update();
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            WaspSpawnPoint waspSpawn = spawnpoint.GetComponent<WaspSpawnPoint>();
            _armorToAttack = waspSpawn.ArmorToAttack;

            _splineYOffset = PartyParrotManager.Instance.Random.NextSingle(-waspSpawn.Offset, waspSpawn.Offset);

            _spline = spawnpoint.GetComponent<BezierSpline>();
            _splineLength = _spline.EstimatedLength();
            _splinePosition = 0.0f;

            _spineAnimationHelper.SetFacing(Vector3.zero - transform.position);

            return true;
        }

        public override void OnDeSpawn()
        {
            _attackCooldownTimer.Stop();

            _attackEndEffect.StopTrigger();
            _attackStartEffect.StopTrigger();

            _spline = null;
            _armorToAttack = null;

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is WaspData);

            base.Initialize(data);

            SetState(State.FollowingSpline);
        }

        public override void Kill(IPlayer player)
        {
            if(null != player) {
                GameManager.Instance.WaspKilled(player);
            }

            base.Kill(player);

            SetState(State.Dead);
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
                _attackStartEffect.Trigger(DoAttackHive);
                break;
            }
        }

        protected override void Think(float dt)
        {
            if(GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            base.Think(dt);

            switch(_state)
            {
            case State.Idle:
                AttackHive();
                break;
            case State.FollowingSpline:
                FollowSpline(dt);
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

#region Actions
        private void AttackHive()
        {
            if(_state == State.Attacking || _attackCooldownTimer.IsRunning) {
                return;
            }

            SetState(State.Attacking);
        }

        private void DoAttackHive()
        {
            _attackEndEffect.Trigger();

            if(_armorToAttack.Damage()) {
                Kill(null);
                return;
            }

            _attackCooldownTimer.Start(WaspData.AttackCooldown);
            SetState(State.Idle);
        }

        private void FollowSpline(float dt)
        {
            float speed = WaspData.Speed;

	        _splinePosition += speed * dt;
	        float t = _splinePosition / _splineLength;
	        Vector3 targetPosition = _spline.GetPoint(t);
            targetPosition.y += _splineYOffset;

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
