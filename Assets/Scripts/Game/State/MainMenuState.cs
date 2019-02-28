using System.Collections.Generic;

using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Input;
using pdxpartyparrot.Game.Menu;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public sealed class MainMenuState : GameState
    {
        [SerializeField]
        private Menu.Menu _menuPrefab;

        private Menu.Menu _menu;

        [SerializeField]
        private AudioClip _music;

        public override IEnumerator<GameStateLoadStatus> OnEnterRoutine()
        {
            yield return new GameStateLoadStatus(0.0f, "Initializing...");

            IEnumerator<GameStateLoadStatus> runner = base.OnEnterRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new GameStateLoadStatus(0.5f, "Initializing...");

            InputManager.Instance.EventSystem.UIModule.EnableAllActions();

            AudioManager.Instance.PlayMusic(_music);

            _menu = UIManager.Instance.InstantiateUIPrefab(_menuPrefab);

            yield return new GameStateLoadStatus(1.0f, "Done!");
        }

        public override IEnumerator<GameStateLoadStatus> OnExitRoutine()
        {
            yield return new GameStateLoadStatus(0.0f, "Shutting down...");

            if(AudioManager.HasInstance) {
                AudioManager.Instance.StopAllMusic();
            }

            if(InputManager.HasInstance) {
                InputManager.Instance.EventSystem.UIModule.DisableAllActions();
            }

            Destroy(_menu.gameObject);
            _menu = null;

            yield return new GameStateLoadStatus(0.5f, "Shutting down...");

            IEnumerator<GameStateLoadStatus> runner = base.OnExitRoutine();
            while(runner.MoveNext()) {
                yield return runner.Current;
            }

            yield return new GameStateLoadStatus(1.0f, "Done!");
        }

        public override void OnResume()
        {
            base.OnResume();

            _menu.gameObject.SetActive(true);
        }

        public override void OnPause()
        {
            _menu.gameObject.SetActive(false);

            base.OnPause();
        }
    }
}
