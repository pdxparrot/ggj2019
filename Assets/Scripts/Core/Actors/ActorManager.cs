﻿using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorManager<T, TV> : SingletonBehavior<TV> where T: Actor where TV: ActorManager<T, TV>
    {
        private readonly List<T> _actors = new List<T>();

        public IReadOnlyCollection<T> Actors => _actors;

        public void Register(T actor)
        {
            _actors.Add(actor);
        }

        public void Unregister(T actor)
        {
            _actors.Remove(actor);
        }
    }
}
