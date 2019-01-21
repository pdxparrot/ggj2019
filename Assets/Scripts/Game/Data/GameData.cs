using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class GameData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _worldLayer;

        public LayerMask WorldLayer => _worldLayer;
    }
}
