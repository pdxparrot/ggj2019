﻿using UnityEngine.Experimental.Input;

namespace pdxpartyparrot.Core.Input
{
    // TODO: I don't think this is used in any way
    public interface ICancelActionHandler
    {
        void OnCancel(InputAction.CallbackContext context);
    }
}
