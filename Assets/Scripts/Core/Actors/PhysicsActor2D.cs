﻿using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PhysicsActor2D : Actor
    {
#region Collider
        public Collider2D Collider { get; private set; }
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider = GetComponent<Collider2D>();
        }
#endregion
    }
}
