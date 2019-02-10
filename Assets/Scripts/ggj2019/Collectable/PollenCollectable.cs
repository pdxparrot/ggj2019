using System;

using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Collectable
{
    [RequireComponent(typeof(PooledObject))]
    public class PollenCollectable : Actor2D
    {
        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;

        [SerializeField]
        [ReadOnly]
        private Players.Player _followPlayer;

        [SerializeField]
        [ReadOnly]
        private bool _isCollected;

        [SerializeField]
        [ReadOnly]
        private float _signTime;

        [SerializeField]
        [ReadOnly]
        private Vector3 _startPoint;

        public override bool IsLocalActor => true;

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

            Float(dt);

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

        public override void OnDeSpawn()
        {
            _followPlayer = null;

            base.OnDeSpawn();
        }

        private void Gather(Players.Player player)
        {
            if(null == player || player.IsDead) {
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
            transform.position = Vector3.Lerp(transform.position, Hive.Instance.transform.position,
                                              GameManager.Instance.GameGameData.CollectableData.GoToHiveSpeed * dt);
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

            // pollen was deposited
            if(!_followPlayer.HasPollen) {
                Collect();
                return true;
            }

            transform.position = Vector3.Lerp(transform.position, _followPlayer.PollenTarget.position,
                                              GameManager.Instance.GameGameData.CollectableData.FollowPlayerSpeed * dt);

            return true;
        }

        private void Float(float dt)
        {
            _signTime += GameManager.Instance.GameGameData.CollectableData.SideSpeed * dt;

            transform.position = new Vector3(_startPoint.x + Mathf.Sin(_signTime) * GameManager.Instance.GameGameData.CollectableData.SideDistance,
                                             transform.position.y + GameManager.Instance.GameGameData.CollectableData.UpwardSpeed * dt);
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
            OnDeSpawn();
        }
#endregion
    }
}
