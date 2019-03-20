using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Splines;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class WaspBehavior : EnemyBehavior
    {
        private enum WaspState
        {
            Idle,
            FollowingSpline,
            Attacking,
            Dead
        }

        [Space(10)]

        [Header("Wasp")]

#region State
        [SerializeField]
        [ReadOnly]
        private WaspState _state = WaspState.Idle;

        public bool IsIdle => WaspState.Idle == _state;

        public override bool IsDead => WaspState.Dead == _state;
#endregion

        [Space(10)]

#region Effects
        [SerializeField]
        private EffectTrigger _attackStartEffect;

        [SerializeField]
        private EffectTrigger _attackEndEffect;
#endregion

        [Space(10)]

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

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private HiveArmor _armorToAttack;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _attackCooldownTimer = new Timer();

        private Wasp WaspNPC => (Wasp)NPC;

        public WaspData WaspData => (WaspData)NPCBehaviorData;

#region Unity Lifecycle
        protected override void Update()
        {
            float dt = Time.deltaTime;

            _attackCooldownTimer.Update(dt);

            base.Update();
        }
#endregion

        public override void Kill(IPlayer player)
        {
            if(null != player) {
                GameManager.Instance.WaspKilled(player);
            }

            base.Kill(player);

            SetState(WaspState.Dead);
        }

        public override void Think(float dt)
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            base.Think(dt);

            switch(_state)
            {
            case WaspState.Idle:
                AttackHive();
                break;
            }
        }

        public override void DefaultPhysicsMove(Vector2 direction, float speed, float dt)
        {
            if(_state != WaspState.FollowingSpline) {
                return;
            }

	        _splinePosition += speed * dt;

	        float t = _splinePosition / _splineLength;

	        Vector3 targetPosition = _spline.GetPoint(t);
            targetPosition.y += _splineYOffset;

SetMoveDirection(targetPosition - Position);
            base.DefaultPhysicsMove(MoveDirection, speed, dt);

            //transform.LookAt2DFlip(targetPosition);

            if(_splinePosition >= _splineLength) {
                SetState(WaspState.Idle);
            }
        }

        private void SetState(WaspState state)
        {
            _state = state;
            switch(state)
            {
            case WaspState.Idle:
                SetIdleAnimation();
                break;
            case WaspState.FollowingSpline:
                SetFlyingAnimation();
                break;
            case WaspState.Attacking:
                _attackStartEffect.Trigger(DoAttackHive);
                break;
            }
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(WaspData.IdleAnimationName, true);
            }
        }

        private void SetFlyingAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(WaspData.FlyingAnimationName, true);
            }
        }
#endregion

#region Actions
        private void AttackHive()
        {
            if(WaspState.Attacking == _state || _attackCooldownTimer.IsRunning) {
                return;
            }

            SetState(WaspState.Attacking);
        }

        private void DoAttackHive()
        {
            _attackEndEffect.Trigger();

            if(_armorToAttack.Damage()) {
                Kill(null);
                return;
            }

            _attackCooldownTimer.Start(WaspData.AttackCooldown);
            SetState(WaspState.Idle);
        }
#endregion

#region Events
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            WaspSpawnPoint waspSpawnPoint = spawnpoint.GetComponent<WaspSpawnPoint>();
            _armorToAttack = waspSpawnPoint.ArmorToAttack;

            _splineYOffset = PartyParrotManager.Instance.Random.NextSingle(-waspSpawnPoint.Offset, waspSpawnPoint.Offset);

            // init our spline state
            _spline = waspSpawnPoint.GetComponent<BezierSpline>();
            _splineLength = _spline.EstimatedLength();
            _splinePosition = 0.0f;

            // teleport to our starting position
            // setting the behavior's Position doesn't seem to immediately warp
            // so falling back on just forcing the transform position to move :(
	        Vector3 targetPosition = _spline.GetPoint(0.0f);
            targetPosition.y += _splineYOffset;
            Teleport(targetPosition);

            if(null != AnimationHelper) {
                AnimationHelper.SetFacing(Vector3.zero - transform.position);
            }

            SetState(WaspState.FollowingSpline);
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
    }
}
