using System;
using System.Text;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="CreditsData", menuName="pdxpartyparrot/Game/Data/Credits Data")]
    [Serializable]
    public sealed class CreditsData : ScriptableObject
    {
        [Serializable]
        public struct Credits
        {
            public string title;

            public string[] contributors;
        }

        [SerializeField]
        [TextArea]
        private string _preAmble;

        [SerializeField]
        private Credits[] _credits;

        [SerializeField]
        [TextArea]
        private string _postAmble;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if(!string.IsNullOrWhiteSpace(_preAmble)) {
                builder.AppendLine(_preAmble);
                builder.AppendLine();
            }

            builder.AppendLine($"<size=36><align=\"center\">{Application.productName}</align></size>");
            builder.AppendLine(); builder.AppendLine();

            foreach(Credits credits in _credits) {
                builder.AppendLine($"<size=24><align=\"center\">{credits.title}</align></size>");
                foreach(string contributor in credits.contributors) {
                    builder.AppendLine($"<size=18><align=\"center\"><pos=5>{contributor}</pos></align></size>");
                }
                builder.AppendLine();
            }

            if(!string.IsNullOrWhiteSpace(_postAmble)) {
                builder.AppendLine(_postAmble);
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
