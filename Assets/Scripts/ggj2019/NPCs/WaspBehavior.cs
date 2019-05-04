using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Splines;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;
using pdxpartyparrot.ggj2019.Players;
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

        private ITimer _attackCooldownTimer;

        private Wasp WaspNPC => (Wasp)NPC;

        public WaspBehaviorData WaspBehaviorData => (WaspBehaviorData)NPCBehaviorData;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _attackCooldownTimer = TimeManager.Instance.AddTimer();
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_attackCooldownTimer);
            }
            _attackCooldownTimer = null;

            base.OnDestroy();
        }
#endregion

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
            case WaspState.FollowingSpline:
                if(_splinePosition >= _splineLength) {
                    SetState(WaspState.Idle);
                }
                break;
            }
        }

        protected override void PhysicsUpdate(float dt)
        {
            base.PhysicsUpdate(dt);

            if(!CanMove || WaspState.FollowingSpline != _state) {
                return;
            }

            _splinePosition += WaspBehaviorData.MoveSpeed * dt;

            float t = _splinePosition / _splineLength;

            Vector3 targetPosition = _spline.GetPoint(t);
            targetPosition.y += _splineYOffset;
            Movement2D.Teleport(targetPosition);
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
            }
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(WaspBehaviorData.IdleAnimationName, true);
            }
        }

        private void SetFlyingAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(WaspBehaviorData.FlyingAnimationName, true);
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

            _attackStartEffect.Trigger(DoAttackHive);
        }

        private void DoAttackHive()
        {
            _attackEndEffect.Trigger();

            if(_armorToAttack.Damage()) {
                WaspNPC.Kill(null);
                return;
            }

            _attackCooldownTimer.Start(WaspBehaviorData.AttackCooldown);
            SetState(WaspState.Idle);
        }
#endregion

#region Events
        public override void OnKill(Player player)
        {
            if(null != player) {
                GameManager.Instance.WaspKilled(player);
            }

            base.OnKill(player);

            SetState(WaspState.Dead);
        }

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
            Movement2D.Teleport(targetPosition);

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
