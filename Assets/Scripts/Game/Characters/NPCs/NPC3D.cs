using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.NPCs
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class NPC3D : Actor3D, INPC
    {
        public GameObject GameObject => gameObject;

#region Network
        public override bool IsLocalActor => false;
#endregion

#region Behavior
        public INPCBehavior NPCBehavior => (NPCBehavior3D)Behavior;
#endregion

        [CanBeNull]
        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is NPCBehavior3D);

            _pooledObject = GetComponent<PooledObject>();
            if(null != _pooledObject) {
                _pooledObject.RecycleEvent += RecycleEventHandler;
            }
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData behaviorData)
        {
            Assert.IsTrue(behaviorData is NPCBehaviorData);

            base.Initialize(id, behaviorData);
        }

        public void Recycle()
        {
            NPCBehavior.OnRecycle();
            if(null != _pooledObject) {
                _pooledObject.Recycle();
            }
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
