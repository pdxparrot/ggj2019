using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Data;

using TMPro;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    // TODO: be sweet if the credits auto-scrolled
    public sealed class CreditsMenu : MenuPanel, ICancelActionHandler
    {
        [SerializeField]
        private CreditsData _creditsData;

        [SerializeField]
        private TextMeshProUGUI _creditsText;

        [SerializeField]
        private ScrollRect _scrollRect;

        [SerializeField]
        private float _scrollRate = 100.0f;

#region Unity Lifecycle
        private void Awake()
        {
            _creditsText.richText = true;
            _creditsText.text = _creditsData.ToString();
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

/*
        // TODO: this isn't working
        private void Update()
        {
            float dt = Time.deltaTime;
            if(_scrollRect.verticalNormalizedPosition < 1.0f) {
                _scrollRect.verticalNormalizedPosition = Mathf.Lerp(_scrollRect.verticalNormalizedPosition, 1.0f, _scrollRate * dt);;
            }
        }
*/
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
