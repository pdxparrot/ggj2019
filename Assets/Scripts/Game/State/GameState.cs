﻿using System;

using pdxpartyparrot.Core.Scenes;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public interface IGameState
    {
        string Name { get; }

        void OnEnter();

        void OnExit();

        void OnResume();

        void OnPause();

        void OnUpdate(float dt);
    }

    public abstract class GameState : MonoBehaviour, IGameState
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

        public void LoadScene(Action callback)
        {
            if(!HasScene) {
                callback?.Invoke();
                return;
            }

            SceneManager.Instance.LoadScene(SceneName, callback, MakeSceneActive);
        }

        public void UnloadScene(Action callback)
        {
            if(!HasScene) {
                callback?.Invoke();
                return;
            }

            if(SceneManager.HasInstance) {
                SceneManager.Instance.UnloadScene(SceneName, callback);
            }
        }

        public virtual void OnEnter()
        {
            Debug.Log($"Enter State: {Name}");
        }

        public virtual void OnExit()
        {
            Debug.Log($"Exit State: {Name}");
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
