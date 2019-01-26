using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Core.Input
{
    public interface ICancelActionHandler
    {
        void OnCancel(InputAction.CallbackContext context);
    }
}
