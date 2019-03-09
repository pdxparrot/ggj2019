using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [CreateAssetMenu(fileName="JumpBehaviorComponentData", menuName="pdxpartyparrot/Game/Data/Behavior Components/JumpBehaviorComponent Data")]
    [Serializable]
    public class JumpBehaviorComponentData : ScriptableObject
    {
        [SerializeField]
        [Range(0, 100)]
        [Tooltip("How high does the character jump")]
        private float _jumpHeight = 30.0f;

        public float JumpHeight => _jumpHeight;

        [Space(10)]

#region Animations
        [Header("Animations")]

        [SerializeField]
        private string _jumpParam = "Jump";

        public string JumpParam => _jumpParam;
#endregion
    }
}