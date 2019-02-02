using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.NPCs.Control;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCBee : NPCBase, ISwarmable
    {
        // TODO: NPCManager.Bees
        private static readonly List<NPCBee> _bees = new List<NPCBee>();

        public static IReadOnlyCollection<NPCBee> Bees => _bees;

        private enum NPCBeeState
        {
            Idle,
            Follow,
        }

        [SerializeField]
        private float _swarmSpeedModifier = 2.0f;

        [SerializeField]
        [ReadOnly]
        private Vector2 _offsetChangeTimer = new Vector2(0.2f,0.5f);

        private float _offsetRadius;
        private Vector2 _offsetPosition = new Vector3(0, 0);
        private readonly Timer _offsetUpdateTimer = new Timer();

        [SerializeField]
        [ReadOnly]
        private NPCBeeState _state = NPCBeeState.Idle;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Swarm _targetSwarm;

        public bool IsInSwarm => null != _targetSwarm;

        public bool CanJoinSwarm => !IsInSwarm && _state == NPCBeeState.Idle;

#region Unity Life Cycle
        private void Update()
        {
            if(IsDead) {
                return;
            }

            float dt = Time.deltaTime;

            _offsetUpdateTimer.Update(dt);

            Think(dt);
        }
#endregion

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            _bees.Add(this);

            _offsetUpdateTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_offsetChangeTimer.x,_offsetChangeTimer.y), UpdateOffset);

            _animation.Skeleton.ScaleX = transform.position.x > 0 ? 1.0f : -1.0f;

            SetHoverAnimation();
        }

        protected override void OnDeSpawn()
        {
            _bees.Remove(this);

            base.OnDeSpawn();
        }

        // start true to force the animation the first time
        private bool _isFlying = true;

        private void SetHoverAnimation()
        {
            if(!_isFlying) {
                return;
            }

            SetAnimation("bee_hover", true);
            _isFlying = false;
        }

        private void SetFlightAnimation()
        {
            if(_isFlying) {
                return;
            }

            SetAnimation("bee-flight", true);
            _isFlying = true;
        }

        private void UpdateOffset()
        {
            _offsetPosition = new Vector2(
                Random.Range(-_offsetRadius, _offsetRadius),
                Random.Range(-_offsetRadius, _offsetRadius)
            );

            _offsetUpdateTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_offsetChangeTimer.x,_offsetChangeTimer.y), UpdateOffset);
        }

        private void Think(float dt)
        {
            // TODO: but still let them flock, that's cool looking
            if(IsDead || GameManager.Instance.IsGameOver || PartyParrotManager.Instance.IsPaused) {
                return;
            }

            switch(_state)
            {
            case NPCBeeState.Idle:
                break;
            case NPCBeeState.Follow:
                Swarm(dt);
                break;
            }
        }

        public void JoinSwarm(Swarm swarm, float radius)
        {
            if(!CanJoinSwarm) {
                return;
            }

            _targetSwarm = swarm;
            SetState(NPCBeeState.Follow);

            _offsetRadius = radius;
        }

        private void SetState(NPCBeeState state)
        {
            //Debug.Log($"setting state: {state}");
            _state = state;

            switch(state)
            {
            case NPCBeeState.Idle:
                SetHoverAnimation();
                break;
            case NPCBeeState.Follow:
                SetFlightAnimation();
                break;
            }
        }

        private float CurrentSpeed()
        {
            float modifier = 1.0f;
            if(IsInSwarm) {
                modifier = _swarmSpeedModifier;
            }

            return PlayerManager.Instance.PlayerData.PlayerControllerData.MoveSpeed * modifier;
        }

#region the things they bee doing
        private void Swarm(float dt)
        {
            if(null == _targetSwarm) {
                Debug.LogWarning("lost my swarm!");
                SetState(NPCBeeState.Idle);
                return;
            }

            MoveToTarget(dt, _targetSwarm.transform);
        }

        private void MoveToTarget(float dt, Transform target)
        {
            if(null == target) {
                return;
            }

            Vector2 position = target.position;
            if(IsInSwarm) {
                position += _offsetPosition;
            }

            transform.position = Vector2.MoveTowards(transform.position,position, CurrentSpeed() * dt);

            SetFlightAnimation();

            Vector2 direction = target.position - transform.position;
            _animation.Skeleton.ScaleX = direction.x < 0 ? -1.0f : 1.0f;
        }
#endregion
    }
}
