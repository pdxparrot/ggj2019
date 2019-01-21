using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    public sealed class UIManager : SingletonBehavior<UIManager>
    {
        private GameObject _uiContainer;

#region Unity Lifecycle
        private void Awake()
        {
            _uiContainer = new GameObject("UI");
        }

        private void OnDestroy()
        {
            Destroy(_uiContainer);
            _uiContainer = null;

            base.OnDestroy();
        }
#endregion

        public TV InstantiateUIPrefab<TV>(TV prefab) where TV: Component
        {
            return Instantiate(prefab, _uiContainer.transform);
        }
    }
}
