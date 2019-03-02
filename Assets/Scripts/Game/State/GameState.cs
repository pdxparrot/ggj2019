using System.Collections.Generic;

using pdxpartyparrot.Core.Loading;
using pdxpartyparrot.Core.Scenes;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class GameState : MonoBehaviour
    {
        public string Name => name;

        [SerializeField]
        private string _sceneName;

        public string SceneName
        {
            get => _sceneName;
            protected set => _sceneName = value;
        }

        public bool HasScene => !string.IsNullOrWhiteSpace(SceneName);

        [SerializeField]
        private bool _makeSceneActive;

        public bool MakeSceneActive => _makeSceneActive;

        public IEnumerator<float> LoadSceneRoutine()
        {
            if(!HasScene) {
                yield break;
            }

            IEnumerator<float> runner = SceneManager.Instance.LoadSceneRoutine(SceneName, MakeSceneActive);
            while(runner.MoveNext()) {
                yield return runner.Current;
            }
        }

        public IEnumerator<float> UnloadSceneRoutine()
        {
            if(!HasScene) {
                yield break;
            }

            if(SceneManager.HasInstance) {
                IEnumerator<float> runner = SceneManager.Instance.UnloadSceneRoutine(SceneName);
                while(runner.MoveNext()) {
                    yield return runner.Current;
                }
            }
        }

        public virtual IEnumerator<LoadStatus> OnEnterRoutine()
        {
            Debug.Log($"Enter State: {Name}");

            yield break;
        }

        public virtual IEnumerator<LoadStatus> OnExitRoutine()
        {
            Debug.Log($"Exit State: {Name}");

            yield break;
        }

        public virtual void OnResume()
        {
            Debug.Log($"Resume State: {Name}");
        }

        public virtual void OnPause()
        {
            Debug.Log($"Pause State: {Name}");
        }

        public virtual void OnUpdate(float dt)
        {
        }
    }
}
