using System;

using UnityEngine;

namespace pdxpartyparrot.Game.Data
{
    [Serializable]
    public abstract class GameData : ScriptableObject
    {
        [SerializeField]
        private LayerMask _viewerLayer;

        public LayerMask ViewerLayer => _viewerLayer;

        [SerializeField]
        private LayerMask _worldLayer;

        public LayerMask WorldLayer => _worldLayer;

        [Space(10)]

        // TODO: this isn't quite the right name for this
        // and probably also not the right place to store it
        [SerializeField]
        [Tooltip("The orthographic size of the 2D camera, which is also the height of the viewport.")]
        private float _gameSize2D = 10;

        public float GameSize2D => _gameSize2D;

        [Space(10)]

        [SerializeField]
        private int _maxLocalPlayers = 1;

        public int MaxLocalPlayers => _maxLocalPlayers;

        [SerializeField]
        private bool _gamepadsArePlayers;

        public bool GamepadsArePlayers => _gamepadsArePlayers;
    }
}
