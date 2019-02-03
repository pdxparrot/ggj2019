﻿using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
    public static class CollectionExtensions
    {
        [CanBeNull]
        public static T GetRandomEntry<T>(this IReadOnlyCollection<T> collection)
        {
            return PartyParrotManager.Instance.Random.GetRandomEntry(collection);
        }

        [CanBeNull]
        public static T RemoveRandomEntry<T>(this IList<T> collection)
        {
            return PartyParrotManager.Instance.Random.RemoveRandomEntry(collection);
        }

        [CanBeNull]
        public static T Nearest<T>(this IReadOnlyCollection<T> collection, Vector3 position) where T: Component
        {
            int bestIdx = -1;
            float bestDistance = float.PositiveInfinity;

            for(int i=0; i<collection.Count; ++i) {
                T element = collection.ElementAt(i);

                float dist = Vector3.Distance(element.transform.position, position);
                if(dist < bestDistance) {
                    bestDistance = dist;
                    bestIdx = i;
                }
            }

            return bestIdx < 0 ? null : collection.ElementAt(bestIdx);
        }

        [CanBeNull]
        public static T Furthest<T>(this IReadOnlyCollection<T> collection, Vector3 position) where T: Component
        {
            int bestIdx = -1;
            float bestDistance = float.NegativeInfinity;

            for(int i=0; i<collection.Count; ++i) {
                T element = collection.ElementAt(i);

                float dist = Vector3.Distance(element.transform.position, position);
                if(dist > bestDistance) {
                    bestDistance = dist;
                    bestIdx = i;
                }
            }

            return bestIdx < 0 ? null : collection.ElementAt(bestIdx);
        }
    }
}