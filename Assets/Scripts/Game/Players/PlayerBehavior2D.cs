using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Game.Players
{
    public abstract class PlayerBehavior2D : CharacterBehavior2D, IPlayerBehavior
    {
        [Space(10)]

        [SerializeField]
        [FormerlySerializedAs("_playerControllerData")]
        private PlayerBehaviorData _playerBehaviorData;

        public PlayerBehaviorData PlayerBehaviorData => _playerBehaviorData;

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
