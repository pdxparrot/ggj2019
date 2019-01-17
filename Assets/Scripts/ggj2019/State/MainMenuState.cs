using pdxpartyparrot.ggj2019.Menu;
using pdxpartyparrot.ggj2019.UI;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.GameState
{
    public sealed class MainMenuState : Game.State.GameState
    {
        [SerializeField]
        private Game.Menu.Menu _menuPrefab;

        private Game.Menu.Menu _menu;

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
