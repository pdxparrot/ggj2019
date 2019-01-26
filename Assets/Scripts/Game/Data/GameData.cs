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

        [SerializeField]
        private float _gameSize2D = 10;

        public float GameSize2D => _gameSize2D;
    }
}
