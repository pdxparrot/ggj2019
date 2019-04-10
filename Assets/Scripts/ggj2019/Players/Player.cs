using System;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Collectables;
using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Players
{
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
        private Swarm _swarm;

        public Swarm Swarm => _swarm;

        public bool IsDead => GamePlayerBehavior.IsDead;

        public bool CanGather => !GamePlayerBehavior.IsDead && (GamePlayerBehavior.GamePlayerBehaviorData.MaxPollen < 0 || GamePlayerBehavior.PollenCount < GamePlayerBehavior.GamePlayerBehaviorData.MaxPollen);

        public int SkinIndex => NetworkPlayer.playerControllerId;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(PlayerBehavior is PlayerBehavior);
            Assert.IsTrue(PlayerDriver is PlayerDriver);

            _skinHelper = GetComponent<SpineSkinHelper>();
        }
#endregion

        protected override bool InitializeLocalPlayer(Guid id)
        {
            if(!base.InitializeLocalPlayer(id)) {
                return false;
            }

            PlayerViewer = GameManager.Instance.Viewer;
            GamePlayerBehavior.InitializeEffects();

            _skinHelper.SetSkin(SkinIndex);

            return true;
        }

        public void AddPollen(Pollen pollen)
        {
            GamePlayerBehavior.OnAddPollen(pollen);
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

#region Spawn
        public override void OnDeSpawn()
        {
            Swarm.RemoveAll();

            base.OnDeSpawn();
        }
#endregion
    }
}
