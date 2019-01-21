using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Menu
{
// TODO: this should populate some sort of UI from the credits data

    public sealed class CreditsMenu : MenuPanel
    {
        [SerializeField]
        private CreditsData _creditsData;

#region Event Handlers
        public void OnBack()
        {
            Owner.PopPanel();
        }
#endregion
    }
}
