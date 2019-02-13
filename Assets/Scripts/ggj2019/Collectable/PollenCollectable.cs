using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

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

        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;

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

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            NPCFlower flower = spawnpoint.GetComponentInParent<NPCFlower>();
            Assert.IsTrue(flower.CanSpawnPollen);
            flower.SpawnPollen();

            SetState(State.Floating);

            return true;
        }
#endregion

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
            if(PartyParrotManager.Instance.IsPaused || GameManager.Instance.IsGameOver) {
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
            case State.Collected:
                // nothing
                break;
            }
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
            _signTime += GameManager.Instance.GameGameData.CollectableData.SideSpeed * dt;
            float wobble = Mathf.Sin(_signTime) * GameManager.Instance.GameGameData.CollectableData.SideDistance;

            position.x = _floatingStartX + wobble;
            position.y += GameManager.Instance.GameGameData.CollectableData.UpwardSpeed * dt;
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
            position = Vector3.Lerp(position, _followPlayer.PollenTarget.position, GameManager.Instance.GameGameData.CollectableData.FollowPlayerSpeed * dt);
            transform.position = position;
        }

        private void GoToHive(float dt)
        {
            if(Hive.Instance.Collides(this)) {
                Collect();
                return;
            }

            Vector3 position = transform.position;
            position = Vector3.Lerp(position, Hive.Instance.transform.position, GameManager.Instance.GameGameData.CollectableData.GoToHiveSpeed * dt);
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
