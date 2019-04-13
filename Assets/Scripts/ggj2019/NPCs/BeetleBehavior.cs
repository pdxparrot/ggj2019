using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Characters.Players;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class BeetleBehavior : EnemyBehavior
    {
        private enum BeetleState
        {
            Idle,
            Attacking,
            Dead
        }

        [Space(10)]

        [Header("Beetle")]

#region State
        [SerializeField]
        [ReadOnly]
        private BeetleState _state = BeetleState.Idle;

        public override bool IsDead => BeetleState.Dead == _state;
#endregion

        [Space(10)]

#region Effects
        [SerializeField]
        private EffectTrigger _attackStartEffect;

        [SerializeField]
        private EffectTrigger _attackEndEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private Flower _flower;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _attackCooldownTimer = new Timer();

        private Beetle BeetleNPC => (Beetle)NPC;

        public BeetleBehaviorData BeetleBehaviorData => (BeetleBehaviorData)NPCBehaviorData;

#region Unity Lifecycle
        protected override void Update()
        {
            float dt = Time.deltaTime;

            _attackCooldownTimer.Update(dt);

            base.Update();
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
            case BeetleState.Idle:
                AttackFlower();
                break;
            }
        }

        private void SetState(BeetleState state)
        {
            _state = state;
            switch(state)
            {
            case BeetleState.Idle:
                SetIdleAnimation();
                break;
            }
        }

#region Actions
        private void AttackFlower()
        {
            if(BeetleState.Attacking == _state || _attackCooldownTimer.IsRunning) {
                return;
            }

            SetState(BeetleState.Attacking);

            _attackStartEffect.Trigger(DoAttackFlower);
        }

        private void DoAttackFlower()
        {
            _attackEndEffect.Trigger();

            _flower.BeetleHarvest(BeetleNPC);
            if(_flower.IsDead) {
                BeetleNPC.Kill(null);
                return;
            }

            _attackCooldownTimer.Start(BeetleBehaviorData.AttackCooldown);
            SetState(BeetleState.Idle);
        }
#endregion

#region Animations
        private void SetIdleAnimation()
        {
            AnimationHelper.SetAnimation(BeetleBehaviorData.IdleAnimation, true);
        }
#endregion

#region Events
        public override void OnKill(IPlayer player)
        {
            if(null != player) {
                GameManager.Instance.BeetleKilled(player);
            }

            base.OnKill(player);

            SetState(BeetleState.Dead);
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            _flower = spawnpoint.GetComponentInParent<Flower>();
            Assert.IsFalse(_flower.IsDead);
            _flower.AcquirePollenSpawnpoint(BeetleNPC);

            SetState(BeetleState.Idle);

            _attackCooldownTimer.Start(BeetleBehaviorData.AttackCooldown);
        }

        public override void OnDeSpawn()
        {
            if(_flower != null) {
                _flower.ReleasePollenSpawnpoint();
                _flower = null;
            }

            _attackCooldownTimer.Stop();

            _attackEndEffect.StopTrigger();
            _attackStartEffect.StopTrigger();

            base.OnDeSpawn();
        }
#endregion
    }
}
