using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class Bee : NPC2D, ISwarmable, IInteractable
    {
        private enum State
        {
            Idle,
            Follow,
            Dead
        }

#region Swarm
        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Swarm _targetSwarm;

        [SerializeField]
        [ReadOnly]
        private float _offsetRadius;

        [SerializeField]
        [ReadOnly]
        private Vector3 _offsetPosition = new Vector3(0.0f, 0.0f);

        public bool IsInSwarm => null != _targetSwarm;

        public bool CanJoinSwarm => !IsDead && !IsInSwarm && _state == State.Idle;
#endregion

#region Interactable
        public Transform Transform => transform;

        public bool CanInteract => CanJoinSwarm;
#endregion

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        public override bool IsDead => _state == State.Dead;

        private BeeData BeeData => (BeeData)NPCData;

        private SpineSkinSwapper _skinSwapper;

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _skinSwapper = GetComponent<SpineSkinSwapper>();

            _spineAnimationHelper.SetFacing(Vector3.zero - transform.position);

            return true;
        }

        public override void OnDeSpawn()
        {
            _targetSwarm = null;

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is BeeData);

            base.Initialize(data);

            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);

            SetState(State.Idle);
        }

        public override void Kill(bool playerKill)
        {
            base.Kill(playerKill);

            SetState(State.Dead);
        }

        private void SetState(State state)
        {
            _state = state;
            switch(state)
            {
            case State.Idle:
                _targetSwarm = null;
                SetIdleAnimation();
                break;
            case State.Follow:
                SetFlyingAnimation();
                break;
            }
        }

        protected override void Think(float dt)
        {
            // it's ok for us to think if the game is over or paused,
            // so that way the swarm movement still happens
            /*if(GameManager.Instance.IsGameOver || PartyParrotManager.Instance.IsPaused) {
                return;
            }*/

            base.Think(dt);

            switch(_state)
            {
            case State.Follow:
                Swarm(dt);
                break;
            }
        }

#region Animation / Skin
        public void SetDefaultSkin()
        {
            _skinSwapper.SetSkin(0);
        }

        public void SetRandomSkin()
        {
            _skinSwapper.SetRandomSkin();
        }

        public void SetPlayerSkin(Players.Player player)
        {
            _skinSwapper.SetSkin(player.SkinIndex);
        }

        private void SetIdleAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeeData.IdleAnimationName, true);
        }

        private void SetFlyingAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeeData.FlyingAnimationName, true);
        }
#endregion

#region Swarm
        private void UpdateSwarmOffset()
        {
            if(IsDead) {
                return;
            }

            _offsetPosition = new Vector2(
                PartyParrotManager.Instance.Random.NextSingle(-_offsetRadius, _offsetRadius),
                PartyParrotManager.Instance.Random.NextSingle(-_offsetRadius, _offsetRadius)
            );
            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);
        }

        public void JoinSwarm(Swarm swarm, float radius)
        {
            if(!CanJoinSwarm) {
                return;
            }

            _offsetRadius = radius;

            _targetSwarm = swarm;
            SetState(State.Follow);
        }

        public void RemoveFromSwarm()
        {
            Kill(false);
        }
#endregion

#region Actions
        private void Swarm(float dt)
        {
            float speed = PlayerManager.Instance.PlayerData.PlayerControllerData.MoveSpeed * BeeData.SwarmSpeedModifier;

            Vector3 swarmPosition = _targetSwarm.transform.position;
            Vector3 targetPosition = swarmPosition + _offsetPosition;

            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * dt);
            transform.position = newPosition;

            _spineAnimationHelper.SetFacing(swarmPosition - newPosition);
        }
#endregion
    }
}
