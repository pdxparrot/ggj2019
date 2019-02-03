using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;
using pdxpartyparrot.ggj2019.NPCs;
using pdxpartyparrot.ggj2019.NPCs.Control;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    [RequireComponent(typeof(SpineSkinSwapper))]
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Collider.bounds.size.y;

        public override float Radius => GamePlayerController.Collider.bounds.size.x / 2.0f;

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

        [SerializeField]
        [ReadOnly]
        private bool _hasPollen;

        public bool HasPollen => _hasPollen;

        private readonly Timer _deathTimer = new Timer();

        private Swarm _swarm;

        private SpineSkinSwapper _skinSwapper;

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
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            if(HasPollen && Hive.Instance.Collides(this)) {
                Hive.Instance.UnloadPollen(this, 1);
                _hasPollen = false;
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
        public void AddPollen()
        {
            _hasPollen = true;
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

            if(!_swarm.HasSwarm) {
                Kill();
                return true;
            }

            _swarm.Remove(amount);
            return true;
        }

        private void Kill()
        {
            _isDead = true;

            _hasPollen = false;

            _swarm.RemoveAll();

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
            if(npcBee != null) {
                AddBeeToSwarm(npcBee);
            }
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
