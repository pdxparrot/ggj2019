using UnityEngine;
using UnityEngine.UI;

namespace pdxpartyparrot.Core.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectAutoScroll : MonoBehaviour
    {
        [SerializeField]
        private float _scrollRate = 100.0f;

        private ScrollRect _scrollRect;

#region Unity Lifecycle
        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

// TODO: this doesn't work
/*
        private void Update()
        {
            float dt = Time.deltaTime;

            // TODO: this sucks because we want it based on the size of the content, not the % scrolled
            // otherwise the speed is going to scale with the length of the content
            if(_scrollRect.verticalNormalizedPosition < 1.0f) {
                _scrollRect.verticalNormalizedPosition = Mathf.Lerp(_scrollRect.verticalNormalizedPosition, 1.0f, _scrollRate * dt);;
            }
        }
*/
#endregion
    }
}
