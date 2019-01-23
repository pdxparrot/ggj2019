#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.Data;

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

            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;
        }
#endregion

        [Server]
        public virtual void ResetPlayer(PlayerData playerData)
        {
            if(null != Player && null != Player.Controller) {
                Player.Controller.Mass = playerData.Mass;
                Player.Controller.LinearDrag = playerData.Drag;
                Player.Controller.AngularDrag = playerData.AngularDrag;
            }
        }

// TODO: we could make better use of NetworkBehaviour callbacks in here (and in other NetworkBehaviour types)

#region Callbacks
        [ClientRpc]
        public virtual void RpcSpawn(int id)
        {
            if(null != Player) {
                Player.Initialize(id);
            }
        }
#endregion
    }
}
