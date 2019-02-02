using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Collider.bounds.size.y / 2.0f;

        public override float Radius => GamePlayerController.Collider.bounds.size.x / 2.0f;

        [SerializeField] private float _harvestRadius;

        [SerializeField]
        private Interactables _interactables;

        [SerializeField]
        private float _respawnTime = 3f;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead => _isDead;

        [SerializeField]
        private EffectTrigger _respawnEffect;

        [SerializeField]
        private EffectTrigger _deathEffect;

        private readonly Timer _deathTimer = new Timer();

        private Swarm _swarm;

        private SpineSkinSwapper _skinSwapper;

        private int _pollen;
        public bool HasPollen { get { return _pollen > 0; } }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _swarm = GetComponent<Swarm>();
            _skinSwapper = GetComponent<SpineSkinSwapper>();

            Assert.IsTrue(PlayerController is PlayerController);
        }

        private void Update()
        {
            /*if(!HasPollen) {
                var flower = NPCFlower.Nearest(transform.position, _harvestRadius);
                if(flower && flower.HasPollen)
                    _pollen += flower.Harvest();
            }
            else {*/
            if(HasPollen) {
                var hive = Hive.Nearest(transform.position);
                if(hive && hive.Collides(this)) {
                    _pollen -= hive.UnloadPollen(this, _pollen);
                }
            }

            float dt = Time.deltaTime;

            _deathTimer.Update(dt);
        }
#endregion

        protected override void InitializeViewer()
        {
            PlayerViewer = GameManager.Instance.Viewer;
        }

        protected override void InitializeModel()
        {
            _skinSwapper.SetSkin(NetworkPlayer.playerControllerId);
        }

#region Actions
        public void AddPollen(int amt)
        {
            _pollen += amt;
        }

        public void AddBeeToSwarm(NPCBee npcBee)
        {
            _swarm.Add(npcBee);
        }


        public bool Damage(int amount)
        {
            if(IsDead) {
                return false;
            }

            if(!_swarm.HasSwarm()) {
                Kill();
                return true;
            }

            _swarm.Kill(amount);
            return true;
        }

        private void Kill()
        {
            _isDead = true;

            _pollen = 0;

            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(true);
            Model.gameObject.SetActive(false);
            _deathEffect.Trigger(() => {    
                _deathTimer.Start(_respawnTime, Respawn);
            });

            GameManager.Instance.PlayerDeath();
        }

        public void DoGather()
        {
            NPCBee npcBee = _interactables.GetBee();
            if (npcBee != null) {
                _swarm.Add(npcBee);
            }
        }

        public void DoContext()
        {
            if(!_swarm.HasSwarm()) {
                return;
            }

            _swarm.DoContext();
        }

        private void Respawn()
        {
            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(false);

            if(GameManager.Instance.IsGameOver) {
                return;
            }

            _isDead = false;

            PlayerManager.Instance.RespawnPlayer(this);

            // TODO: maybe we only show the model after the effect ends?
            _respawnEffect.Trigger();
            Model.gameObject.SetActive(true);
        }
#endregion
    }
}
