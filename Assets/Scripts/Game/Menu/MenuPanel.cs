using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Core.UI;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField]
        private Menu _owner;

        protected Menu Owner => _owner;

        [SerializeField]
        private Button _initialSelection;

#region Unity Lifecycle
        private void Awake()
        {
            if(null == _initialSelection) {
                Debug.LogWarning("MenuPanel missing initial selection");
            } else {
                _initialSelection.Select();
                _initialSelection.Highlight();
            }
        }

        private void Update()
        {
#if UNITY_EDITOR	
            if(null == _initialSelection) {	
                return;	
            }	
#endif	

            if(null == InputManager.Instance.EventSystem.EventSystem.currentSelectedGameObject ||
                (!InputManager.Instance.EventSystem.EventSystem.currentSelectedGameObject.activeInHierarchy && _initialSelection.gameObject.activeInHierarchy)) {
                _initialSelection.Select();
                _initialSelection.Highlight();
            }
        }
#endregion

        public void ResetMenu()
        {
            Debug.LogWarning($"TODO: reset menu {name}");

            _initialSelection.Select();
            _initialSelection.Highlight();
        }
    }
}
