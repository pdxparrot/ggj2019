using pdxpartyparrot.Game.Menu;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Menu
{
    public sealed class MainMenu : MenuPanel
    {
#region Event Handlers
        public void OnQuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
#endregion
    }
}
