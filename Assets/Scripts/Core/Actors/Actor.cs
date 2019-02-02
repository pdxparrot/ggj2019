using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _id = -1;

        public int Id => _id;

        public string Name => name;

        public Vector3 Position => transform.position;

        public abstract float Height { get; }

        public abstract float Radius { get; }

        [CanBeNull]
        public virtual Viewer Viewer { get; set; }

#region Model
        [CanBeNull]
        [SerializeField]
        private GameObject _model;

        [CanBeNull]
        public GameObject Model => _model;
#endregion

#region Controller
        [CanBeNull]
        [SerializeField]
        private ActorController _controller;

        [CanBeNull]
        public ActorController Controller => _controller;
#endregion

#region Animation
        [CanBeNull]
        [SerializeField]
        private Animator _animator;

        [CanBeNull]
        public Animator Animator => _animator;
#endregion

        public abstract bool IsLocalActor { get; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

        public virtual void Initialize(int id)
        {
            _id = id;
        }

#region Callbacks
        public virtual void OnSpawn(SpawnPoint spawnpoint)
        {
        }

        public virtual void OnReSpawn(SpawnPoint spawnpoint)
        {
        }
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            if(Animator != null) {
                Animator.enabled = !PartyParrotManager.Instance.IsPaused;
            }
        }
#endregion
    }
}
