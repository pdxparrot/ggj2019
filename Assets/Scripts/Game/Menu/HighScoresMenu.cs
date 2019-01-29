using pdxpartyparrot.Core.Input;

using TMPro;

using UnityEngine;
using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class HighScoresMenu : MenuPanel, ICancelActionHandler
    {
        [SerializeField]
        private TextMeshProUGUI _highScoresText;

#region Unity Lifecycle
        private void Awake()
        {
            _highScoresText.richText = true;
            _highScoresText.text = HighScoreManager.Instance.HighScoresText();
        }

        private void OnEnable()
        {
            InputManager.Instance.EventSystem.UIModule.cancel.action.performed += OnCancel;
        }

        private void OnDisable()
        {
            if(InputManager.HasInstance) {
                InputManager.Instance.EventSystem.UIModule.cancel.action.performed -= OnCancel;
            }
        }
#endregion

#region Event Handlers
        public void OnBack()
        {
            Owner.PopPanel();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            OnBack();
        }
#endregion
    }
}
