using pdxpartyparrot.Core.Audio;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonHelper : MonoBehaviour, ISelectHandler, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField]
        private AudioClip _buttonHoverAudioClip;

        [SerializeField]
        private AudioClip _buttonClickAudioClip;

        private Button _button;

#region Unity Lifecycle
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
#endregion

#region Event Handlers
        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(null == _buttonHoverAudioClip ? PartyParrotManager.Instance.UIData.ButtonHoverAudioClip : _buttonHoverAudioClip);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _button.Select();
            _button.Highlight();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(null == _buttonClickAudioClip ? PartyParrotManager.Instance.UIData.ButtonClickAudioClip : _buttonClickAudioClip);
        }
#endregion
    }
}
