using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Game.Collectables;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;

using TMPro;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Collectables
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pollen : Actor2D, ICollectable
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
        private TextMeshPro _collectText;

        [SerializeField]
        [ReadOnly]
        private float _floatingStartX;

        [SerializeField]
        [ReadOnly]
        private Players.Player _followPlayer;

        [SerializeField]
        [ReadOnly]
        private Hive _hive;

        [SerializeField]
        [ReadOnly]
        private float _signTime;

        [SerializeField]
        [ReadOnly]
        private State _state = State.Floating;

        [SerializeField]
        [ReadOnly]
        private PollenData _pollenData;

        private Rigidbody2D _rigidbody;

        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0.0f;

            Collider.isTrigger = true;

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            Think(dt);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Gather(other.gameObject.GetComponent<Players.Player>());
            Collect(other.gameObject.GetComponent<Hive>());
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Collect(other.gameObject.GetComponent<Hive>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Gather(other.gameObject.GetComponent<Players.Player>());
            Collect(other.gameObject.GetComponent<Hive>());
        }
#endregion

        public void Initialize(CollectableData data)
        {
            Assert.IsTrue(data is PollenData);

            _pollenData = (PollenData)data;

            SetState(State.Floating);
        }

        private void SetState(State state)
        {
            _state = state;
            switch(_state)
            {
            case State.Floating:
                _collectText.gameObject.SetActive(true);
                _floatingStartX = transform.position.x;
                _followPlayer = null;
                _hive = null;
                break;
            case State.FollowingPlayer:
                _collectText.gameObject.SetActive(false);
                break;
            case State.GoingToHive:
                _collectText.gameObject.SetActive(false);
                break;
            case State.Collected:
                _collectText.gameObject.SetActive(false);
                _followPlayer = null;
                _hive = null;
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

        public void Unload(Hive hive)
        {
            _hive = hive;

            SetState(State.GoingToHive);
        }

        private void Gather(Players.Player player)
        {
            if(!CanBeCollected || null == player || !player.CanGather) {
                return;
            }

            player.AddPollen(this);
            _pickupEffect.Trigger();

            _followPlayer = player;
            SetState(State.FollowingPlayer);
        }

        private void Collect(Hive hive)
        {
            if(null == hive || _state != State.GoingToHive) {
                return;
            }

            hive.CollectPollen(_followPlayer);

            SetState(State.Collected);

            _collectEffect.Trigger(() => {
                _pooledObject.Recycle();
            });
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
            _rigidbody.MovePosition(position);
        }

        private void FollowPlayer(float dt)
        {
            // if the player is missing or dead, we float
            if(_followPlayer == null || _followPlayer.IsDead) {
                SetState(State.Floating);
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, _followPlayer.PollenTarget.position, _pollenData.FollowPlayerSpeed * dt);
            _rigidbody.MovePosition(position);
        }

        private void GoToHive(float dt)
        {
            if(_hive == null) {
                SetState(State.Floating);
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, _hive.transform.position, _pollenData.GoToHiveSpeed * dt);
            _rigidbody.MovePosition(position);
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
