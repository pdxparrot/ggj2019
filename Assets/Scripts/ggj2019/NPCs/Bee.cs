using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Players;

using TMPro;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
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
        private TextMeshPro _gatherText;

        [SerializeField]
        [ReadOnly]
        private State _state = State.Idle;

        public bool IsDead => _state == State.Dead;

        private BeeData BeeData => (BeeData)NPCData;

        private SpineAnimationHelper _spineAnimationHelper;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is BeeBehavior);

            _spineAnimationHelper = GetComponent<SpineAnimationHelper>();
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _skinHelper = GetComponent<SpineSkinHelper>();

            _spineAnimationHelper.SetFacing(Vector3.zero - transform.position);

            return true;
        }

        public override void OnDeSpawn()
        {
            _targetSwarm = null;

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(Guid id, NPCData data)
        {
            Assert.IsTrue(data is BeeData);

            base.Initialize(id, data);

            TimeManager.Instance.RunAfterDelay(BeeData.OffsetUpdateRange.GetRandomValue(), UpdateSwarmOffset);

            SetState(State.Idle);
        }

        public override void Kill(IPlayer player)
        {
            base.Kill(player);

            SetState(State.Dead);
        }

        private void SetState(State state)
        {
            _state = state;
            switch(state)
            {
            case State.Idle:
                _gatherText.gameObject.SetActive(true);
                _targetSwarm = null;
                SetIdleAnimation();
                break;
            case State.Follow:
                _gatherText.gameObject.SetActive(false);
                SetFlyingAnimation();
                break;
            case State.Dead:
                _gatherText.gameObject.SetActive(false);
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
            _skinHelper.SetSkin(0);
        }

        public void SetRandomSkin()
        {
            _skinHelper.SetRandomSkin();
        }

        public void SetPlayerSkin(Players.Player player)
        {
            _skinHelper.SetSkin(player.SkinIndex);
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
            Kill(null);
        }
#endregion

#region Actions
        private void Swarm(float dt)
        {
            // TODO: don't do this every frame :(
            IPlayer player = _targetSwarm.GetComponent<IPlayer>();
            float speed = player.PlayerBehavior.PlayerBehaviorData.MoveSpeed * BeeData.SwarmSpeedModifier;

            Vector3 swarmPosition = _targetSwarm.transform.position;
            Vector3 targetPosition = swarmPosition + _offsetPosition;

            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * dt);
            transform.position = newPosition;

            _spineAnimationHelper.SetFacing(swarmPosition - newPosition);
        }
#endregion
    }
}
