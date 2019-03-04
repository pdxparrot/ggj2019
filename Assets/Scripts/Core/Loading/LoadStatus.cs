using System;

namespace pdxpartyparrot.Core.Loading
{
    [Serializable]
    public class LoadStatus
    {
        public float LoadPercent { get; }

        public string Status { get; }

        public LoadStatus(float loadPercent, string status)
        {
            LoadPercent = loadPercent;
            Status = status;
        }
    }
}
