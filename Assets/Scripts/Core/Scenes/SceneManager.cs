using System;
using System.Collections;
using System.Collections.Generic;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace pdxpartyparrot.Core.Scenes
{
    public sealed class SceneManager : SingletonBehavior<SceneManager>
    {
        [SerializeField]
        private string _mainSceneName = "main";

        private readonly List<string> _loadedScenes = new List<string>();

#region Unity Lifecycle
        private void Awake()
        {
            InitDebugMenu();
        }
#endregion

#region Load Scene
        public void SetScene(string sceneName)
        {
            Debug.Log($"Setting scene '{sceneName}'");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(string sceneName, Action callback, bool setActive=false)
        {
            StartCoroutine(LoadSceneRoutine(sceneName, () => {
                callback?.Invoke();
            }, setActive));
        }

        public IEnumerator LoadSceneRoutine(string sceneName, Action callback, bool setActive=false)
        {
            Debug.Log($"Loading scene '{sceneName}'");

            AsyncOperation asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while(!asyncOp.isDone) {
                yield return null;
            }

            if(setActive) {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
            }

            _loadedScenes.Add(sceneName);

            callback?.Invoke();
        }
#endregion

#region Unload Scene
        public void UnloadScene(string sceneName, Action callback)
        {
            StartCoroutine(UnloadSceneRoutine(sceneName, () => {
                callback?.Invoke();
            }));
        }

        public IEnumerator UnloadSceneRoutine(string sceneName, Action callback)
        {
            IEnumerator runner = DoUnloadSceneRoutine(sceneName, callback);
            while(runner.MoveNext()) {
                yield return null;
            }
            _loadedScenes.Remove(sceneName);
        }

        private IEnumerator DoUnloadSceneRoutine(string sceneName, Action callback)
        {
            Debug.Log($"Unloading scene '{sceneName}'");

            AsyncOperation asyncOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            while(!asyncOp.isDone) {
                yield return null;
            }

            callback?.Invoke();
        }

        public IEnumerator UnloadAllScenesRoutine(Action callback)
        {
            Debug.Log("Unloading all scenes");

            foreach(string sceneName in _loadedScenes) {
                IEnumerator runner = DoUnloadSceneRoutine(sceneName, null);
                while(runner.MoveNext()) {
                    yield return null;
                }
            }
            _loadedScenes.Clear();

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(_mainSceneName));

            callback?.Invoke();
        }
#endregion

#region Reload Scene
        public void ReloadMainScene()
        {
            Debug.Log("Reloading...");
            StartCoroutine(UnloadAllScenesRoutine(() => {
                Debug.Log($"Loading main scene '{_mainSceneName}'");
                UnityEngine.SceneManagement.SceneManager.LoadScene(_mainSceneName);
            }));
        }

        public void ReloadScene(string sceneName, Action callback)
        {
            Debug.Log($"Reloading scene '{sceneName}'");

            UnloadScene(sceneName, () => {
                LoadScene(sceneName, callback);
            });
        }
#endregion

        private void InitDebugMenu()
        {
            DebugMenuNode debugMenuNode = DebugMenuManager.Instance.AddNode(() => "Core.SceneManager");
            debugMenuNode.RenderContentsAction = () => {
                GUILayout.BeginVertical("Loaded Scenes", GUI.skin.box);
                    foreach(string loadedScene in _loadedScenes) {
                        GUILayout.Label(loadedScene);
                    }
                GUILayout.EndVertical();
            };
        }
    }
}
