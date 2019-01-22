#pragma warning disable 0618    // disable obsolete warning for now

using JetBrains.Annotations;

using pdxpartyparrot.Core.Network;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Game.Players
{
    [RequireComponent(typeof(NetworkAnimator))]
    public abstract class NetworkPlayer : NetworkActor
    {
        [CanBeNull]
        public Player Player => (Player)Actor;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            NetworkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            NetworkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisY;
        }
#endregion

        [Server]
        public virtual void ResetPlayer(PlayerData playerData)
        {
            if(null != Player) {
                Player.Controller.Rigidbody.mass = playerData.Mass;
                Player.Controller.Rigidbody.drag = playerData.Drag;
                Player.Controller.Rigidbody.angularDrag = playerData.AngularDrag;
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
