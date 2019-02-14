using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game
{
    public interface IGameManager
    {
        GameData GameData { get; }

        int MaxLocalPlayers { get; }

        bool GamepadsArePlayers { get; }

        bool IsGameOver { get; }
    }

    public abstract class GameManager<T> : SingletonBehavior<T>, IGameManager where T: GameManager<T>
    {
        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        [SerializeField]
        private int _maxLocalPlayers = 1;

        public int MaxLocalPlayers => _maxLocalPlayers;

        [SerializeField]
        private bool _gamepadsArePlayers;

        public bool GamepadsArePlayers => _gamepadsArePlayers;

        public abstract bool IsGameOver { get; protected set; }
    }
}
