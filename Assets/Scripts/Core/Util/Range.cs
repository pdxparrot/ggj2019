using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Util
{
// TODO: these can't have setters because we use them in data
// so we probably need a separate RangeData class for that

    // TODO: rename IntRange
    [Serializable]
    public struct Range
    {
        [SerializeField]
        private int _min;

        public int Min => _min;

        [SerializeField]
        private int _max;

        public int Max => _max;

        public Range(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public int GetRandomValue()
        {
            return PartyParrotManager.Instance.Random.Next(_min, _max);
        }

        // rounds down
        public int GetPercentValue(float pct)
        {
            pct = Mathf.Clamp01(pct);
            return (int)(_min + (pct * (_max - _min)));
        }
    }

    [Serializable]
    public struct FloatRange
    {
        [SerializeField]
        private float _min;

        public float Min => _min;

        [SerializeField]
        private float _max;

        public float Max => _max;

        public FloatRange(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public float GetRandomValue()
        {
            return PartyParrotManager.Instance.Random.NextSingle(_min, _max);
        }

        public float GetPercentValue(float pct)
        {
            pct = Mathf.Clamp01(pct);
            return _min + (pct * (_max - _min));
        }
    }
}
