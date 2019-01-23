using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    public sealed class LocalizationManager : SingletonBehavior<LocalizationManager>
    {
        [SerializeField]
        private LocalizationData _localizationData;

        public string GetText(string id)
        {
            return $"Missing text {id}";
        }
    }
}
