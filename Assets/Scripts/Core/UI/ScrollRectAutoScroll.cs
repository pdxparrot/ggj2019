﻿using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
/*
Tips on making this work:

Add a LayoutGroup to Content
    Set Child Controls Size Width / Height
    Set Child Force Expand Width / Height
Add a ContentSizeFitter to Content
    Set Horizontal Fit to Unconstrained
    Set Vertical Fit to Preferred Size
Delete the ScrollBars completely to get them to not show up


TODO: for some reason the first time this is enabled, the size of the container is wrong
and setting the scroll position to the top isn't working
*/

    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectAutoScroll : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Delay before starting to scroll")]
        private float _delay = 1.0f;

        [SerializeField]
        [Tooltip("Units per-second to scroll")]
        private float _scrollRate = 100.0f;

        [SerializeField]
        private bool _resetOnEnable = true;

        [SerializeField]
        [ReadOnly]
        private float _step;

        [SerializeField]
        [ReadOnly]
        private float _delayRemaining;

        [SerializeField]
        [ReadOnly]
        private float _lastScrollAmount;

        [SerializeField]
        [ReadOnly]
        private float _lastVerticalPosition;

        private ScrollRect _scrollRect;

#region Unity Lifecycle
        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _scrollRect.verticalNormalizedPosition = 1.0f;
        }

        private void OnEnable()
        {
            _step = Mathf.Clamp01(_scrollRate / _scrollRect.content.rect.height);
            if(_resetOnEnable) {
                _scrollRect.verticalNormalizedPosition = 1.0f;
            }

            _delayRemaining = _delay;
        }

        private void Update()
        {
            float dt = UnityEngine.Time.deltaTime;

            Scroll(dt);
        }
#endregion

        private void Scroll(float dt)
        {
            // delay
            if(_delayRemaining > 0.0f) {
                _delayRemaining -= dt;
                return;
            }

            // don't scroll past the bottom
            if(_scrollRect.verticalNormalizedPosition <= 0.0f) {
                _lastScrollAmount = 0.0f;
                _lastVerticalPosition = _scrollRect.verticalNormalizedPosition = 0.0f;
                return;
            }

            _lastScrollAmount = _step * dt;

            _scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(_scrollRect.verticalNormalizedPosition, 0.0f, _lastScrollAmount);
            _lastVerticalPosition = _scrollRect.verticalNormalizedPosition;
        }
    }
}
