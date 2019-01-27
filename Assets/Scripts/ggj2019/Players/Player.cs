using pdxpartyparrot.Game.Players;
using pdxpartyparrot.Game.UI;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(Swarm))]
    public sealed class Player : Player2D
    {
        public PlayerController GamePlayerController => (PlayerController)PlayerController;

        public override float Height => GamePlayerController.Collider.bounds.size.y;

        public override float Radius => GamePlayerController.Collider.bounds.size.x;

        [SerializeField] private Interactables _interactables;

        private Swarm _swarm;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _swarm = GetComponent<Swarm>();

            Assert.IsTrue(PlayerController is PlayerController);
        }
#endregion

        protected override void InitializeViewer()
        {
            PlayerViewer = GameManager.Instance.Viewer;
        }

        public void CollectResource()
        {
            Debug.LogWarning("got a resource!");
        }

#region Actions
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
                _swarm.DoContext();
                return;
            }

            // No swarm so the player does the thing
        }
#endregion
    }
}
