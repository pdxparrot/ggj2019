using System;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.Core.Data
{
    [CreateAssetMenu(fileName="UIData", menuName="pdxpartyparrot/Core/Data/UI Data")]
    [Serializable]
    public class UIData : ScriptableObject
    {
        [SerializeField]
        private AudioClip _buttonHoverAudioClip;

        public AudioClip ButtonHoverAudioClip => _buttonHoverAudioClip;

        [SerializeField]
        private AudioClip _buttonClickAudioClip;

        public AudioClip ButtonClickAudioClip => _buttonClickAudioClip;

        [SerializeField]
        private TMP_FontAsset _defaultFont = TMP_FontAsset.defaultFontAsset;

        public TMP_FontAsset DefaultFont => _defaultFont;
    }
}
