﻿using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    public abstract class UIManager<T> : SingletonBehavior<T> where T: UIManager<T>
    {
        private GameObject _uiContainer;

#region Unity Lifecycle
        protected virtual void Awake()
        {
            _uiContainer = new GameObject("UI");
        }

        protected override void OnDestroy()
        {
            Destroy(_uiContainer);
            _uiContainer = null;

            base.OnDestroy();
        }
#endregion

        public V InstantiateUIPrefab<V>(V prefab) where V: Component
        {
            return Instantiate(prefab, _uiContainer.transform);
        }
    }
}
