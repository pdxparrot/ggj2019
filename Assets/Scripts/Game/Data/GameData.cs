using System;

using UnityEngine;
using UnityEngine.Serialization;

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

        [Header("Viewport")]

        // TODO: this probably isn't the best way to handle this or the best place to put it
        [SerializeField]
        [FormerlySerializedAs("_gameSize2D")]
        [Tooltip("The orthographic size of the 2D camera, which is also the height of the viewport.")]
        private float _viewportSize = 10;

        public float ViewportSize => _viewportSize;

        [Space(10)]

        [Header("Players")]

        [SerializeField]
        private int _maxLocalPlayers = 1;

        public int MaxLocalPlayers => _maxLocalPlayers;

        [SerializeField]
        private bool _gamepadsArePlayers;

        public bool GamepadsArePlayers => _gamepadsArePlayers;
    }
}
