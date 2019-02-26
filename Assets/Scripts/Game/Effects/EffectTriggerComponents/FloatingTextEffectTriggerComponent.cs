﻿using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Game.UI;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects.EffectTriggerComponents
{
    public class FloatingTextEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private string _poolName = "floating_text";

        [SerializeField]
        private string _text;

        [SerializeField]
        private Color _color;

        public override bool WaitForComplete => false;

        public override void OnStart()
        {
            UIManager.Instance.QueueFloatingText(_poolName, _text, _color, () => transform.position);
        }
    }
}
