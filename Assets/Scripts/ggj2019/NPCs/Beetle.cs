using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    public sealed class Beetle : Enemy
    {
        private enum State
        {
            Idle,
            Attacking,
            Dead
        }

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
        private Flower _flower;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private SpawnPoint _spawnpoint;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _attackCooldownTimer = new Timer();

        private BeetleData BeetleData => (BeetleData)NPCData;

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

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                return false;
            }
            _spawnpoint = spawnpoint;

            _flower = _spawnpoint.GetComponentInParent<Flower>();
            Assert.IsFalse(_flower.IsDead);
            _flower.AcquirePollenSpawnpoint(this);

            return true;
        }

        public override void OnDeSpawn()
        {
            _attackCooldownTimer.Stop();

            _attackEndEffect.StopTrigger();
            _attackStartEffect.StopTrigger();

            if(_flower != null) {
                _flower.ReleasePollenSpawnpoint();
                _flower = null;
            }

            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is BeetleData);

            base.Initialize(data);

            _attackCooldownTimer.Start(BeetleData.AttackCooldown);

            SetState(State.Idle);
        }

        public override void Kill(bool playerKill)
        {
            if(playerKill) {
                GameManager.Instance.BeetleKilled();
            }

            base.Kill(playerKill);

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
            case State.Attacking:
                _attackStartEffect.Trigger(DoAttackFlower);
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
                AttackFlower();
                break;
            }
        }

#region Animations
        private void SetIdleAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeetleData.IdleAnimation, true);
        }
#endregion

#region Actions
        private void AttackFlower()
        {
            if(_state == State.Attacking || _attackCooldownTimer.IsRunning) {
                return;
            }

            SetState(State.Attacking);
        }

        private void DoAttackFlower()
        {
            _attackEndEffect.Trigger();

            if(_flower.BeetleHarvest()) {
                Kill(false);
                return;
            }

            _attackCooldownTimer.Start(BeetleData.AttackCooldown);
            SetState(State.Idle);
        }
#endregion
    }
}
