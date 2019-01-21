﻿using System.Collections.Generic;

using JetBrains.Annotations;

namespace pdxpartyparrot.Core.Util
{
    public static class DictionaryExtensions
    {
        [CanBeNull]
        public static TV GetOrDefault<TK, TV>(this IReadOnlyDictionary<TK, TV> dict, TK key, TV defaultValue=default(TV))
        {
            return dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static bool Remove<TK, TV>(this IDictionary<TK, TV> dict, TK key, out TV value)
        {
            return dict.TryGetValue(key, out value) && dict.Remove(key);
        }
    }
}
