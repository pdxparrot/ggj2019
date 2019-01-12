// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/Controls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Input;


namespace pdxpartyparrot.Core.Input
{
    [Serializable]
    public class Controls : InputActionAssetReference
    {
        public Controls()
        {
        }
        public Controls(InputActionAsset asset)
            : base(asset)
        {
        }
        private bool m_Initialized;
        private void Initialize()
        {
            m_Initialized = true;
        }
        private void Uninitialize()
        {
            m_Initialized = false;
        }
        public void SetAsset(InputActionAsset newAsset)
        {
            if (newAsset == asset) return;
            if (m_Initialized) Uninitialize();
            asset = newAsset;
        }
        public override void MakePrivateCopyOfActions()
        {
            SetAsset(ScriptableObject.Instantiate(asset));
        }
        [Serializable]
        public class ActionEvent : UnityEvent<InputAction.CallbackContext>
        {
        }
    }
}
