using System;
using DG.Tweening;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

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

        public bool IsDead => _isDead;
        private bool _isDead;

        private Timer _deathTimer;
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
            if(IsDead)
                _deathTimer.Update(Time.deltaTime);
        }

        #endregion

        protected override void InitializeViewer()
        {
            PlayerViewer = GameManager.Instance.Viewer;
        }

#region Actions

        public void AddBeeToSwarm(NPCBee npcBee)
        {
            _swarm.Add(npcBee);
        }


        public void Damage(int amount)
        {
            if (!_swarm.HasSwarm())
                PlayerDeath();

            int damageLeft = _swarm.kill(amount);

            if (damageLeft > 0)
            {
                PlayerDeath();
            }

        }

        // Hacky
        private void PlayerDeath()
        {
            _isDead = true;

            PlayDeathFX();

            _deathTimer = new Timer();
            _deathTimer.Start(_respawnTime, Respawn);
        }

        // TODO remove when spawned bees attach to player

        public void DoGather()
        {
            NPCBee npcBee = _interactables.GetBee();
            if (npcBee != null)
                _swarm.Add(npcBee);
        }

        public void DoContext()
        {
            if (_swarm.HasSwarm())
            {
                // No more context
                //_swarm.DoContext();
                return;
            }

            // No swarm so the player does the thing
        }

        private void Respawn()
        {
            PlayerManager.Instance.RespawnPlayer(this);

            PlayReviveFX();

            _isDead = false;
        }

        private void PlayDeathFX()
        {
            //TODO Play Death anim
            Model.gameObject.SetActive(false);
        }

        private void PlayReviveFX()
        {
            //TODO Play revive anim
            Model.gameObject.SetActive(true);
        }

        #endregion
    }
}
