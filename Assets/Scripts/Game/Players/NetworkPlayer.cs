#pragma warning disable 0618    // disable obsolete warning for now

using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Network;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(NetworkAnimator))]
    public abstract class NetworkPlayer : NetworkActor
    {
        [CanBeNull]
        public IPlayer Player => (IPlayer)Actor;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Actor is IPlayer);
        }
#endregion

// TODO: we could make better use of NetworkBehaviour callbacks in here (and in other NetworkBehaviour types)

#region Callbacks
        [ClientRpc]
        public virtual void RpcSpawn(string id)
        {
            if(null != Player) {
                Player.Initialize(Guid.Parse(id));
            }
        }
#endregion
    }
}
