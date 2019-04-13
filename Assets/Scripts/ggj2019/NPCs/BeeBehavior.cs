using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class BeeBehavior : NPCBehavior
    {
        private enum BeeState
        {
            Idle,
            Follow,
            Dead
        }

        [Space(10)]

        [Header("Bee")]

#region State
        [SerializeField]
        [ReadOnly]
        private BeeState _state = BeeState.Idle;

        public bool IsIdle => BeeState.Idle == _state;

        public override bool IsDead => BeeState.Dead == _state;
#endregion

        [Space(10)]

#region Swarm
        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Swarm _targetSwarm;

        public bool HasTargetSwarm => null != _targetSwarm;

        [SerializeField]
        [ReadOnly]
        private float _swarmOffsetRadius;

        [SerializeField]
        [ReadOnly]
        private Vector3 _swarmOffsetPosition = new Vector3(0.0f, 0.0f);
#endregion

        private Bee BeeNPC => (Bee)NPC;

        public BeeBehaviorData BeeBehaviorData => (BeeBehaviorData)NPCBehaviorData;

        protected override void AnimationUpdate(float dt)
        {
            base.AnimationUpdate(dt);

            if(BeeState.Follow != _state) {
                return;
            }

            Vector3 swarmPosition = _targetSwarm.transform.position;
            AnimationHelper.SetFacing(swarmPosition - Position);
        }

        protected override void PhysicsUpdate(float dt)
        {
            base.PhysicsUpdate(dt);

            // swarming while the game is paused is kinda cool
            // so we should allow it
            bool canMove = CanMove || PartyParrotManager.Instance.IsPaused;
            if(!canMove || BeeState.Follow != _state) {
                return;
            }

            Vector3 swarmPosition = _targetSwarm.Center.position;
            Vector3 targetPosition = swarmPosition + _swarmOffsetPosition;
            MoveTowards(targetPosition, BeeBehaviorData.SwarmSpeed, dt);
        }

        private void SetState(BeeState state)
        {
            _state = state;
            switch(_state)
            {
            case BeeState.Idle:
                BeeNPC.EnableGatherText(true);
                _targetSwarm = null;
                SetIdleAnimation();
                break;
            case BeeState.Follow:
                BeeNPC.EnableGatherText(false);
                SetFlyingAnimation();
                break;
            case BeeState.Dead:
                BeeNPC.EnableGatherText(false);
                break;
            }
        }

        private void UpdateSwarmOffset()
        {
            if(IsDead) {
                return;
            }

            _swarmOffsetPosition = new Vector2(
                PartyParrotManager.Instance.Random.NextSingle(-_swarmOffsetRadius, _swarmOffsetRadius),
                PartyParrotManager.Instance.Random.NextSingle(-_swarmOffsetRadius, _swarmOffsetRadius)
            );
            TimeManager.Instance.RunAfterDelay(BeeBehaviorData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);
        }

#region Animation
        private void SetIdleAnimation()
        {
            AnimationHelper.SetAnimation(BeeBehaviorData.IdleAnimationName, true);
        }

        private void SetFlyingAnimation()
        {
            AnimationHelper.SetAnimation(BeeBehaviorData.FlyingAnimationName, true);
        }
#endregion

#region Events
        public override void OnKill(IPlayer player)
        {
            base.OnKill(player);

            SetState(BeeState.Dead);
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            AnimationHelper.SetFacing(Vector3.zero - transform.position);

            SetState(BeeState.Idle);

            TimeManager.Instance.RunAfterDelay(BeeBehaviorData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);
        }

        public override void OnDeSpawn()
        {
            _targetSwarm = null;

            base.OnDeSpawn();
        }

        public void OnJoinSwarm(Swarm swarm, float radius)
        {
            _swarmOffsetRadius = radius;

            _targetSwarm = swarm;
            SetState(BeeState.Follow);
        }

        public void OnRemoveFromSwarm()
        {
            BeeNPC.Kill(null);
        }
#endregion
    }
}
