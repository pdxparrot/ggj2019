using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public sealed class SceneTester : MainGameState
    {
        [SerializeField]
        private string[] _testScenes;

        public string[] TestScenes => _testScenes;

        public void SetScene(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
