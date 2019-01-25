using pdxpartyparrot.Game.Players;

using UnityEngine;

namespace pdxpartyparrot.Game.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class PlayerUI : MonoBehaviour
    {
        private Canvas _canvas;

#region Unity Lifecycle
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
#endregion

        public void Initialize(IPlayer player)
        {
            if(null != player.Viewer) {
                _canvas.worldCamera = player.Viewer.UICamera;
            }
        }
    }
}
