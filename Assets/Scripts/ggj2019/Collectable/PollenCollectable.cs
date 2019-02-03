using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Collectable
{
    [RequireComponent(typeof(PooledObject))]
    public class PollenCollectable: PhysicsActor2D
    {
        [SerializeField]
        private float _sideDistance = 0.25f;

        [SerializeField]
        private float _sideSpeed = 0.5f;

        [SerializeField]
        private float _upwardSpeed = 0.001f;

        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;

        [SerializeField]
        [ReadOnly]
        private Players.Player _followPlayer;

        [SerializeField]
        [ReadOnly]
        private bool _isCollected ;

        [SerializeField]
        [ReadOnly]
        private float _signTime;

        [SerializeField]
        [ReadOnly]
        private Vector3 _startPoint;

        public override float Height => Collider.bounds.size.y;

        public override float Radius => Collider.bounds.size.x / 2.0f;

        public override bool IsLocalActor => true;

        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            float dt = Time.deltaTime;

            if(_isCollected) {
                GoToHive(dt);
                return;
            }

            if(FollowPlayer(dt)) {
                return;
            }

            Move(dt);

            if(transform.position.y  - Height / 2.0f > GameStateManager.Instance.GameManager.GameData.GameSize2D) {
                _pooledObject.Recycle();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_isCollected) {
                return;
            }

            Gather(other.gameObject.GetComponent<Players.Player>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(_isCollected) {
                return;
            }

            Gather(other.gameObject.GetComponent<Players.Player>());
        }
#endregion

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            NPCFlower flower = spawnpoint.GetComponentInParent<NPCFlower>();
            Assert.IsFalse(flower.IsDead);
            flower.SpawnPollen();

            _startPoint = transform.position;
            _isCollected = false;
        }

        private void Gather(Players.Player player)
        {
            if(null == player) {
                return;
            }

            if(player.HasPollen) {
                return;
            }

            _followPlayer = player;

            player.AddPollen();
            _pickupEffect.Trigger();
        }

        private void GoToHive(float dt)
        {
            transform.position = Vector3.Lerp(transform.position, Hive.Instance.Position, 10f * dt);
        }

        private bool FollowPlayer(float dt)
        {
            if(_followPlayer == null) {
                return false;
            }

            if(_followPlayer.IsDead) {
                _followPlayer = null;
                return false;
            }

            transform.position = Vector3.Lerp(transform.position, _followPlayer.Position + new Vector3(0.25f,0.0f), 20.0f * dt);

            // pollen was deposited
            if(!_followPlayer.HasPollen) {
                Collect();
            }

            return true;
        }

        private void Move(float dt)
        {
            _signTime += dt * _sideSpeed;

            transform.position = new Vector3((Mathf.Sin(_signTime) * _sideDistance) + _startPoint.x, transform.position.y + (_upwardSpeed * dt));
        }

        private void Collect()
        {
            _isCollected = true;

            _collectEffect.Trigger(() => {
                _pooledObject.Recycle();
            });
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            _followPlayer = null;
        }
#endregion
    }
}
