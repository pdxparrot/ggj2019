using pdxpartyparrot.Game.Players;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class PlayerUI : MonoBehaviour
    {
        private Canvas _canvas;

        protected Canvas Canvas => _canvas;

#region Unity Lifecycle
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
#endregion

        public virtual void Initialize(UnityEngine.Camera uiCamera)
        {
            _canvas.worldCamera = uiCamera;
        }
    }
}
