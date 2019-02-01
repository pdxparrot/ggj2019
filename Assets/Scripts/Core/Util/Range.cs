using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
// TODO: split this into float and int
    [Serializable]
    public struct Range
    {
        [SerializeField]
        private int _min;

        public int Min
        {
            get => _min;
            set => _min = value;
        }

        [SerializeField]
        private int _max;

        public int Max
        {
            get => _max;
            set => _max = value;
        }

// TODO: GetRandomValue
        public int GetValue()
        {
            return PartyParrotManager.Instance.Random.Next(_min, _max);
        }

// TODO: GetPercentValue (or something)
    }
}
