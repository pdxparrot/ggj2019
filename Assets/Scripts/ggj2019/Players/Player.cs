using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class Player : Player2D
    {
        public PlayerBehavior GamePlayerBehavior => (PlayerBehavior)PlayerBehavior;

        public PlayerDriver GamePlayerDriver => (PlayerDriver)PlayerDriver;

        [SerializeField]
        private Transform _pollenTarget;

        public Transform PollenTarget => _pollenTarget;

        [SerializeField]
        private Interactables _interactables;

        public Interactables Interactables => _interactables;

        [SerializeField]
        [ReadOnly]
        private bool _isDead;

        public bool IsDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ List<Pollen> _pollen = new List<Pollen>();

        public List<Pollen> Pollen => _pollen;

        public bool CanGather => !IsDead && (PlayerManager.Instance.GamePlayerData.MaxPollen < 0 || Pollen.Count < PlayerManager.Instance.GamePlayerData.MaxPollen);

        public int SkinIndex => NetworkPlayer.playerControllerId;

        public Swarm Swarm { get; private set; }

        public SpineSkinHelper SkinHelper { get; private set; }

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
            Assert.IsTrue(PlayerDriver is PlayerDriver);

            Swarm = GetComponent<Swarm>();
            SkinHelper = GetComponent<SpineSkinHelper>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GamePlayerBehavior.OnCollide(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            GamePlayerBehavior.OnCollide(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GamePlayerBehavior.OnCollide(other.gameObject);
        }
#endregion

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;

            SkinHelper.SetSkin(SkinIndex);

            return true;
        }

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            GamePlayerBehavior.OnSpawn();

            return true;
        }

        public override bool OnReSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnReSpawn(spawnpoint)) {
                return false;
            }

            GamePlayerBehavior.OnReSpawn();

            return true;
        }

        public override void OnDeSpawn()
        {
            _pollen.Clear();
            Swarm.RemoveAll();

            GamePlayerBehavior.OnDeSpawn();

            base.OnDeSpawn();
        }
#endregion

        public void AddPollen(Pollen pollen)
        {
            _pollen.Add(pollen);
        }

        public void AddBeeToSwarm(Bee bee)
        {
            Swarm.Add(bee);
            _interactables.RemoveInteractable(bee);

            bee.SetPlayerSkin(this);
        }

        public void Damage()
        {
            GamePlayerBehavior.OnDamage();
        }

        public void GameOver()
        {
            GamePlayerBehavior.OnGameOver();
        }
    }
}
