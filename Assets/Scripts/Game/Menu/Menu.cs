﻿using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Game.Menu
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasScaler))]
    public sealed class Menu : MonoBehaviour
    {
        [SerializeField]
        private MenuPanel _mainPanel;

        [SerializeField]
        [ReadOnly]
        private MenuPanel _currentPanel;

        private readonly Stack<MenuPanel> _panelStack = new Stack<MenuPanel>();

#region Unity Lifecycle
        private void Awake()
        {
            GetComponent<Canvas>().sortingOrder = 100;

            PushPanel(_mainPanel);
        }
#endregion

        public void ResetMenu()
        {
            _currentPanel.ResetMenu();
        }

        public void PushPanel(MenuPanel panel)
        {
            if(null != _currentPanel) {
                _currentPanel.gameObject.SetActive(false);
                _panelStack.Push(_currentPanel);
            }

            _currentPanel = panel;
            _currentPanel.gameObject.SetActive(true);
        }

        public void PopPanel()
        {
            if(_panelStack.Count < 1) {
                return;
            }

            _currentPanel.gameObject.SetActive(false);

            _currentPanel = _panelStack.Pop();
            _currentPanel.gameObject.SetActive(true);
        }
    }
}