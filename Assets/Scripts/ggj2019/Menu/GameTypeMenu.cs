using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.State;

using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.UI;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class GameTypeMenu : MenuPanel, ICancelActionHandler
    {
        [SerializeField]
        private Button _teamsButton;

#region Unity Lifecycle
        private void Awake()
        {
            // disable the team map for now
            if(null != _teamsButton) {
                _teamsButton.gameObject.SetActive(false);
            }
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
        public void OnFreeForAll()
        {
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.FFAGameStatePrefab);
        }

        public void OnTeams()
        {
            GameStateManager.Instance.StartLocal(GameManager.Instance.GameGameData.TeamsGameStatePrefab);
        }

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
