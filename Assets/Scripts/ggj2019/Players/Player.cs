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
        public PlayerController GamePlayerBehavior => (PlayerController)PlayerBehavior;

        private PlayerDriver GamePlayerDriver => (PlayerDriver)PlayerDriver;

        [SerializeField]
        private Transform _pollenTarget;

        public Transform PollenTarget => _pollenTarget;

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

            Assert.IsTrue(PlayerBehavior is PlayerController);
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

            GamePlayerDriver.GamepadListener.Rumble(PlayerManager.Instance.GamePlayerData.DamageRumble);

            _swarm.Remove(amount);
            return true;
        }

        public void GameOver()
        {
            GamePlayerDriver.GamepadListener.Rumble(PlayerManager.Instance.GamePlayerData.GameOverRumble);
        }

        private void Kill()
        {
            GamePlayerDriver.GamepadListener.Rumble(PlayerManager.Instance.GamePlayerData.DeathRumble);

            Behavior.Velocity = Vector3.zero;

            _isDead = true;

            _hasPollen = false;

            _swarm.RemoveAll();

            ((UI.PlayerUI)UIManager.Instance.PlayerUI).ShowDeathText(true);
            Model.gameObject.SetActive(false);
            _deathEffect.Trigger(() => {    
                _deathTimer.Start(_respawnTime, Respawn);
            });

            // despawn the actor (not the player)
            OnDeSpawn();

            GameManager.Instance.PlayerDeath();
        }

        public void DoGather()
        {
            if(IsDead) {
                return;
            }

            NPCBee npcBee = _interactables.GetBee();
            if(npcBee != null) {
                AddBeeToSwarm(npcBee);
            }
        }

        private void Respawn()
        {
            GamePlayerDriver.GamepadListener.Rumble(PlayerManager.Instance.GamePlayerData.RespawnRumble);

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
    }
}
