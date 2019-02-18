using TMPro;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextHelper : MonoBehaviour
    {
        [SerializeField]
        private string _stringId;

#region Unity Lifecycle
        private void Awake()
        {
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
            text.font = PartyParrotManager.Instance.UIData.DefaultFont;
            text.text = LocalizationManager.Instance.GetText(_stringId);
        }
#endregion
    }
}
