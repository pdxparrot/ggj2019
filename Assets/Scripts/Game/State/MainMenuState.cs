using pdxpartyparrot.Core.Audio;
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

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = UIManager.Instance.InstantiateUIPrefab(_menuPrefab);

            AudioManager.Instance.PlayMusic(_music);
        }

        public override void OnExit()
        {
            AudioManager.Instance.StopMusic();

            Destroy(_menu.gameObject);
            _menu = null;

            base.OnExit();
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
