using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Collider.bounds.size.y;

        public override float Radius => GamePlayerController.Collider.bounds.size.x;

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

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _swarm = GetComponent<Swarm>();

            Assert.IsTrue(PlayerController is PlayerController);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _deathTimer.Update(dt);
        }

        #endregion

        protected override void InitializeViewer()
        {
            PlayerViewer = GameManager.Instance.Viewer;
        }

#region Actions

        public void Damage(int amount)
        {
            if(IsDead) {
                return;
            }

            if(!_swarm.HasSwarm()) {
                Kill();
                return;
            }

            _swarm.Kill(amount);
        }

        private void Kill()
        {
            _isDead = true;

            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(true);
            Model.gameObject.SetActive(false);
            _deathEffect.Trigger(() => {    
                _deathTimer.Start(_respawnTime, Respawn);
            });
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
            _isDead = false;

            PlayerManager.Instance.RespawnPlayer(this);

            // TODO: maybe we only show the model after the effect ends?
            _respawnEffect.Trigger();
            Model.gameObject.SetActive(true);
            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(false);
        }
#endregion
    }
}
