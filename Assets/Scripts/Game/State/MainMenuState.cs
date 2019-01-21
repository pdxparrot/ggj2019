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

        public override void OnEnter()
        {
            base.OnEnter();

            _menu = UIManager.Instance.InstantiateUIPrefab(_menuPrefab);
        }

        public override void OnExit()
        {
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
