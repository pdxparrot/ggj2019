﻿using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Collider))]
    public abstract class PhysicsActor3D : Actor
    {
#region Collider
        public Collider Collider { get; private set; }
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider = GetComponent<Collider>();
        }
#endregion
    }
}