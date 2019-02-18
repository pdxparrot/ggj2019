using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game
{
    public interface IGameManager
    {
        GameData GameData { get; }

        bool IsGameOver { get; }

        void Initialize();

        void Shutdown();
    }

    public abstract class GameManager<T> : SingletonBehavior<T>, IGameManager where T: GameManager<T>
    {
        [SerializeField]
        private GameData _gameData;

        public GameData GameData => _gameData;

        public abstract bool IsGameOver { get; protected set; }

        public abstract void Initialize();

        public abstract void Shutdown();
    }
}
