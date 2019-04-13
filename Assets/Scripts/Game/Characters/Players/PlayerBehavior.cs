using pdxpartyparrot.Game.Data;

using UnityEngine;

namespace pdxpartyparrot.Game.Characters.Players
{
    public interface IPlayerBehavior
    {
        PlayerBehaviorData PlayerBehaviorData { get; }

        IPlayer Player { get; }

        Vector2 MoveDirection { get; }

        void SetMoveDirection(Vector2 moveDirection);
    }
}
