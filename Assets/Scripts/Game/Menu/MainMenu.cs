using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
    public sealed class MainMenu : MenuPanel
    {
#region Event Handlers
        public void OnPlay()
        {
            Debug.Log("TODO: Play Game");
        }

        public void OnCredits()
        {
            Debug.Log("TODO: Credits");
        }

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
