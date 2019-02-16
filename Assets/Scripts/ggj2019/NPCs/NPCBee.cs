using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.NPCs.Control;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCBee : NPCBase, ISwarmable, IInteractable
    {
        private enum State
        {
            Idle,
            Follow,
        }

        [SerializeField]
        [ReadOnly]
        private float _offsetRadius;

        [SerializeField]
        [ReadOnly]
        private Vector3 _offsetPosition = new Vector3(0.0f, 0.0f);

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Swarm _targetSwarm;

        private NPCBeeData BeeData => (NPCBeeData)NPCData;

        public Transform Transform => transform;

        public bool IsInSwarm => null != _targetSwarm;

        public bool CanJoinSwarm => !IsDead && !IsInSwarm && _state == State.Idle;

        public bool CanInteract => CanJoinSwarm;

#region Unity Life Cycle
        private void Update()
        {
            float dt = Time.deltaTime;

            Think(dt);
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is NPCBeeData);

            base.Initialize(data);

            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateOffset);

            _spineAnimationHelper.SetFacing(Vector3.zero - transform.position);

            SetState(State.Idle);
        }

#region Animation
        private void SetHoverAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeeData.HoverAnimationName, true);
        }

        private void SetFlightAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeeData.FlightAnimationName, true);
        }
#endregion

        private void UpdateOffset()
        {
            if(IsDead) {
                return;
            }

            _offsetPosition = new Vector2(
                PartyParrotManager.Instance.Random.NextSingle(-_offsetRadius, _offsetRadius),
                PartyParrotManager.Instance.Random.NextSingle(-_offsetRadius, _offsetRadius)
            );
            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateOffset);
        }

        private void Think(float dt)
        {
            // it's ok for us to think if the game is over or paused,
            // so that way the swarm movement still happens
            if(IsDead /*|| GameManager.Instance.IsGameOver || PartyParrotManager.Instance.IsPaused*/) {
                return;
            }

            switch(_state)
            {
            case State.Idle:
                break;
            case State.Follow:
                Swarm(dt);
                break;
            }
        }

        private void SetState(State state)
        {
            _state = state;
            switch(state)
            {
            case State.Idle:
                _targetSwarm = null;
                SetHoverAnimation();
                break;
            case State.Follow:
                SetFlightAnimation();
                break;
            }
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

        public override void Kill(bool playerKill)
        {
            _targetSwarm = null;

            base.Kill(playerKill);
        }

        private float CurrentSpeed()
        {
            float modifier = 1.0f;
            if(IsInSwarm) {
                modifier = BeeData.SwarmSpeedModifier;
            }

            return PlayerManager.Instance.PlayerData.PlayerControllerData.MoveSpeed * modifier;
        }

        private void Swarm(float dt)
        {
            if(null == _targetSwarm) {
                Debug.LogWarning("lost my swarm!");
                SetState(State.Idle);
                return;
            }

            MoveToTarget(dt, _targetSwarm.transform);
        }

        private void MoveToTarget(float dt, Transform target)
        {
            if(null == target) {
                return;
            }

            Vector3 position = target.position;
            if(IsInSwarm) {
                position += _offsetPosition;
            }

            position = Vector3.MoveTowards(transform.position,position, CurrentSpeed() * dt);
            _spineAnimationHelper.SetFacing(target.position - position);
            transform.position = position;
        }
    }
}
