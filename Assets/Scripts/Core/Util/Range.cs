using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
// TODO: split this into float and int
// TODO: this can't have setters because we use it in data
// so we probably need a separate RangeData class for that
    [Serializable]
    public struct Range
    {
        [SerializeField]
        private int _min;

        public int Min => _min;

        [SerializeField]
        private int _max;

        public int Max => _max;

// TODO: GetRandomValue
        public int GetValue()
        {
            return PartyParrotManager.Instance.Random.Next(_min, _max);
        }

// TODO: GetPercentValue (or something)
    }
}
