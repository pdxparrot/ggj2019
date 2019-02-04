using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    // TODO: the way this works (using a dictionary of types)
    // could be applied to the ViewerManager
    public sealed class ActorManager : SingletonBehavior<ActorManager>
    {
        private readonly Dictionary<Type, HashSet<Actor>> _actors = new Dictionary<Type, HashSet<Actor>>();

        public void Register<T>(T actor) where T: Actor
        {
            Type actorType = actor.GetType();
            //Debug.Log($"Registering actor {actor.Id} of type {actorType}");

            HashSet<Actor> actors = _actors.GetOrAdd(actorType);
            actors.Add(actor);
        }

        public void Unregister<T>(T actor) where T: Actor
        {
            Type actorType = actor.GetType();
            //Debug.Log($"Unregistering actor {actor.Id} of type {actorType}");

            if(_actors.TryGetValue(actorType, out var actors)) {
                actors.Remove(actor);
            }
        }

        public int ActorCount<TV>() where TV: Actor
        {
            return _actors.TryGetValue(typeof(TV), out var actors) ? actors.Count : 0;
        }

        public IReadOnlyCollection<Actor> GetActors<T>() where T: Actor
        {
            return _actors.GetOrDefault(typeof(T));
        }
    }
}
