using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Game.Players
{
    // TODO: reduce the copy paste in this
    public abstract class PlayerController2D : CharacterBehavior2D, IPlayerController
    {
        [SerializeField]
        [FormerlySerializedAs("_playerControllerData")]
        private PlayerBehaviorData _behaviorData;

        public PlayerBehaviorData PlayerBehaviorData => _behaviorData;

        public IPlayer Player => (IPlayer)Owner;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Owner is IPlayer);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // fixes sketchy rigidbody angular momentum shit
            AngularVelocity2D = 0;
        }
#endregion
    }
}
