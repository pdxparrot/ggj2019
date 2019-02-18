using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Collectable
{
    [RequireComponent(typeof(PooledObject))]
    public class PollenCollectable : Actor2D, ICollectable
    {
        private enum State
        {
            Floating,
            FollowingPlayer,
            GoingToHive,
            Collected
        }

        public override bool IsLocalActor => false;

        public bool CanBeCollected => _state == State.Floating;

#region Effects
        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;
#endregion

        [SerializeField]
        [ReadOnly]
        private float _floatingStartX;

        [SerializeField]
        [ReadOnly]
        private Players.Player _followPlayer;

        [SerializeField]
        [ReadOnly]
        private float _signTime;

        [SerializeField]
        [ReadOnly]
        private State _state = State.Floating;

        [SerializeField]
        [ReadOnly]
        private CollectableData _pollenData;

        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider.isTrigger = true;

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            Think(dt);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Gather(other.gameObject.GetComponent<Players.Player>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Gather(other.gameObject.GetComponent<Players.Player>());
        }
#endregion

        public void Initialize(CollectableData data)
        {
            _pollenData = data;

            SetState(State.Floating);
        }

        private void SetState(State state)
        {
            _state = state;
            switch(_state)
            {
            case State.Floating:
                _floatingStartX = transform.position.x;
                _followPlayer = null;
                break;
            case State.GoingToHive:
                _followPlayer = null;
                break;
            case State.Collected:
                _followPlayer = null;
                break;
            }
        }

        private void Think(float dt)
        {
            // it's ok for us to think if the game is over,
            // so that way the float still happens
            if(PartyParrotManager.Instance.IsPaused /*|| GameManager.Instance.IsGameOver*/) {
                return;
            }

            switch(_state)
            {
            case State.Floating:
                Float(dt);
                break;
            case State.FollowingPlayer:
                FollowPlayer(dt);
                break;
            case State.GoingToHive:
                GoToHive(dt);
                break;
            }
        }

        private void Gather(Players.Player player)
        {
            if(!CanBeCollected || null == player || !player.CanGather) {
                return;
            }

            player.AddPollen();
            _pickupEffect.Trigger();

            _followPlayer = player;
            SetState(State.FollowingPlayer);
        }

        private void Float(float dt)
        {
            // recycle if we're off the screen
            Vector3 position = transform.position;
            if(position.y - Height / 2.0f > GameStateManager.Instance.GameManager.GameData.GameSize2D) {
                _pooledObject.Recycle();
                return;
            }

            // float side-to-side
            _signTime += _pollenData.SideSpeed * dt;
            float wobble = Mathf.Sin(_signTime) * _pollenData.SideDistance;

            position.x = _floatingStartX + wobble;
            position.y += _pollenData.UpwardSpeed * dt;
            transform.position = position;
        }

        private void FollowPlayer(float dt)
        {
            // if the player is missing or dead, we float
            if(_followPlayer == null || _followPlayer.IsDead) {
                SetState(State.Floating);
                return;
            }

            // pollen was deposited, head to the hive
            if(!_followPlayer.HasPollen) {
                SetState(State.GoingToHive);
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, _followPlayer.PollenTarget.position, _pollenData.FollowPlayerSpeed * dt);
            transform.position = position;
        }

        private void GoToHive(float dt)
        {
            if(Hive.Instance.Collides(this)) {
                Collect();
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, Hive.Instance.transform.position, _pollenData.GoToHiveSpeed * dt);
            transform.position = position;
        }

        private void Collect()
        {
            SetState(State.Collected);

            _collectEffect.Trigger(() => {
                _pooledObject.Recycle();
            });
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
