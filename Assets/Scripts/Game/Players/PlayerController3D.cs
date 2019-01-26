﻿using pdxpartyparrot.Game.Actors;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Players
{
    // TODO: reduce the copy paste in this
    public abstract class PlayerController3D : CharacterActorController3D, IPlayerController
    {
        [SerializeField]
        private PlayerControllerData _playerControllerData;

        public PlayerControllerData PlayerControllerData => _playerControllerData;

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
            Rigidbody.angularVelocity = Vector3.zero;
        }
#endregion
    }
}