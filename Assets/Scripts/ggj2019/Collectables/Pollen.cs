using System;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Collectables;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;

using TMPro;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.Collectables
{
    [RequireComponent(typeof(PooledObject))]
    public class Pollen : Actor2D, ICollectable
    {
#region Actor
        public override bool IsLocalActor => false;
#endregion

#region ICollectable
        public bool CanBeCollected => PollenBehavior.IsFloating;

        [SerializeField]
        [ReadOnly]
        private PollenData _pollenData;
#endregion

        [SerializeField]
        private TextMeshPro _collectText;

        private PollenBehavior PollenBehavior => (PollenBehavior)Behavior;

        private PooledObject _pooledObject;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(Behavior is PollenBehavior);

            Collider.isTrigger = true;

            _pooledObject = GetComponent<PooledObject>();
            _pooledObject.RecycleEvent += RecycleEventHandler;
        }
#endregion

        public void Initialize(CollectableData collectableData)
        {
            Assert.IsTrue(collectableData is PollenData);

            _pollenData = (PollenData)collectableData;
        }

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is PollenBehaviorData);

            base.Initialize(id, data);
        }

        public void EnableCollectText(bool enable)
        {
            _collectText.gameObject.SetActive(enable);
        }

        public void Unload(Hive hive)
        {
            PollenBehavior.OnUnload(hive);
        }

        public void Recycle()
        {
            _pooledObject.Recycle();
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            OnDeSpawn();
        }
#endregion
    }
}
