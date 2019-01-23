using pdxpartyparrot.Core.Audio;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonHelper : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _buttonHoverAudioClip;

        [SerializeField]
        private AudioClip _buttonClickAudioClip;

#region Unity Lifecycle
        private void Awake()
        {
            Button button = GetComponent<Button>();
            InitHoverAudio(button);
            InitClickAudio(button);
        }
#endregion

        private void InitHoverAudio(Button button)
        {
            EventTrigger hoverTrigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(data => OnPointerEnter((PointerEventData)data));
            hoverTrigger.triggers.Add(entry);
        }

        private void InitClickAudio(Button button)
        {
            button.onClick.AddListener(OnClick);
        }

#region Event Handlers
        private void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot(null == _buttonHoverAudioClip ? PartyParrotManager.Instance.UIData.ButtonHoverAudioClip : _buttonHoverAudioClip);
        }

        private void OnClick()
        {
            AudioManager.Instance.PlayOneShot(null == _buttonClickAudioClip ? PartyParrotManager.Instance.UIData.ButtonClickAudioClip : _buttonClickAudioClip);
        }
#endregion
    }
}
