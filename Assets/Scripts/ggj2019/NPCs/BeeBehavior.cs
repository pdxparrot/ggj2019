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

        public BeeData BeeData => (BeeData)NPCBehaviorData;

        public override void Kill(IPlayer player)
        {
            base.Kill(player);

            SetState(BeeState.Dead);
        }

        public override void DefaultAnimationMove(Vector2 direction, float dt)
        {
            if(null == _targetSwarm) {
                base.DefaultAnimationMove(direction, dt);
                return;
            }

            Vector3 swarmPosition = _targetSwarm.transform.position;

            if(null != AnimationHelper) {
                AnimationHelper.SetFacing(swarmPosition - Position);
            }
        }

        public override void DefaultPhysicsMove(Vector2 direction, float speed, float dt)
        {
            if(_state != BeeState.Follow) {
                return;
            }

            if(null == _targetSwarm) {
                SetState(BeeState.Idle);
                return;
            }

            Vector3 swarmPosition = _targetSwarm.transform.position;
            Vector3 targetPosition = swarmPosition + _swarmOffsetPosition;

SetMoveDirection(targetPosition - Position);
            base.DefaultPhysicsMove(MoveDirection, BeeData.SwarmMoveSpeed, dt);
        }

        private void SetState(BeeState state)
        {
            _state = state;
            switch(state)
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
            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);
        }

#region Animation
        private void SetIdleAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(BeeData.IdleAnimationName, true);
            }
        }

        private void SetFlyingAnimation()
        {
            if(null != AnimationHelper) {
                AnimationHelper.SetAnimation(BeeData.FlyingAnimationName, true);
            }
        }
#endregion

#region Events
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            if(null != AnimationHelper) {
                AnimationHelper.SetFacing(Vector3.zero - transform.position);
            }

            SetState(BeeState.Idle);

            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);
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
            Kill(null);
        }
#endregion
    }
}
