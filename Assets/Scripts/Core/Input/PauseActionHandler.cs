using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Core.Input
{
    public interface IPauseActionHandler
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
