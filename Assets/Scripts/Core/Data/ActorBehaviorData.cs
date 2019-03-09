using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Data
{
    [Serializable]
    public abstract class ActorBehaviorData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _actorLayer;

        public LayerMask ActorLayer => _actorLayer;

#region Physics
        [Header("Physics")]

        [SerializeField]
        private float _mass = 45.0f;

        public float Mass => _mass;

        [SerializeField]
        private float _drag = 0.0f;

        public float Drag => _drag;

        [SerializeField]
        private float _angularDrag = 0.0f;

        public float AngularDrag => _angularDrag;

        [SerializeField]
        private bool _isKinematic = false;

        public bool IsKinematic => _isKinematic;
#endregion
    }
}
