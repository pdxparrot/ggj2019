using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerController : CharacterActorController
    {
        [SerializeField]
        private PlayerControllerData _playerControllerData;

        public PlayerControllerData PlayerControllerData => _playerControllerData;

        public Player Player => (Player)Owner;

#region Unity Lifecycle
        protected override void FixedUpdate()
        {
            // fixes sketchy rigidbody angular momentum shit
            Rigidbody.angularVelocity = Vector3.zero;
        }
#endregion
    }
}
