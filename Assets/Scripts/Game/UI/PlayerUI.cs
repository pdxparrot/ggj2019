using pdxpartyparrot.Game.Players;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    public abstract class PlayerUI : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        public virtual void Initialize(UnityEngine.Camera uiCamera)
        {
            _canvas.worldCamera = uiCamera;
        }
    }
}
