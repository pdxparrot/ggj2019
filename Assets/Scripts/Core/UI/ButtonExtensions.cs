using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
    // TODO: is this useful to non-button Selectables?
    public static class ButtonExtensions
    {
        public static void Highlight(this Button button)
        {
            button.OnSelect(null);
        }
    }
}
