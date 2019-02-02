using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Collectable
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(PooledObject))]
    public class PollenCollectable: MonoBehaviour
    {
        [SerializeField]
        private float _sideDistance = 0.25f;

        [SerializeField]
        private float _sideSpeed = 0.5f;

        [SerializeField]
        private float _upwardSpeed = 0.001f;

        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;

        [SerializeField]
        [ReadOnly]
        private int _pollen = 1;

        [SerializeField]
        [ReadOnly]
        private Players.Player followPlayer;

        [SerializeField]
        [ReadOnly]
        private bool _isCollected ;

        [SerializeField]
        [ReadOnly]
        private float _signTime;

        [SerializeField]
        [ReadOnly]
        private Vector3 _startPoint;

        private Hive _hive;

        private Collider2D _collider;

#region Unity Lifecycle
        private void Awake()
        {
            _startPoint = transform.position;
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if(PartyParrotManager.Instance.IsPaused) {
                return;
            }

            if(_isCollected) {
                if(!_particleSystem.isPlaying) {
                    Destroy(gameObject);
                }

                GoToHive();
                return;
            }

            if(FollowPlayer()) {
                return;
            }

            Move(Time.deltaTime);

            if(transform.position.y  - _collider.bounds.size.y / 2.0f > GameStateManager.Instance.GameManager.GameData.GameSize2D) {
                Destroy(gameObject);
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

        private void Gather(Players.Player player)
        {
            if(null == player) {
                return;
            }

            if(player.HasPollen) {
                return;
            }

            followPlayer = player;

            player.AddPollen(_pollen);
            _pickupEffect.Trigger();
        }

        private void GoToHive()
        {
            transform.position = Vector3.Lerp(transform.position, _hive.Position, 10f * Time.deltaTime);
        }

        private bool FollowPlayer()
        {
            if(followPlayer == null) {
                return false;
            }

            if(followPlayer.IsDead) {
                followPlayer = null;
                return false;
            }

            transform.position = Vector3.Lerp(transform.position, followPlayer.Position + new Vector3(0.25f,0f), 20f*Time.deltaTime);

            // pollen was deposited
            if(!followPlayer.HasPollen) {
                Collect();
            }

            return true;
        }

        private void Move(float dt)
        {
            _signTime += dt * _sideSpeed;

            transform.position = new Vector3((Mathf.Sin(_signTime) * _sideDistance) + _startPoint.x,
                transform.position.y + (_upwardSpeed * dt));
        }

        public void SetPollenAmt(int amt)
        {
            _pollen = amt;
        }

        private void Collect()
        {
            _isCollected = true;
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _hive = Hive.Nearest(transform.position);

            _collectEffect.Trigger(() => {
                Destroy(gameObject);
            });
        }
    }
}
