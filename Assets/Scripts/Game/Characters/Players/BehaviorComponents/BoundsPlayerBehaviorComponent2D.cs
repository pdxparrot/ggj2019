using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Game.Characters.Players.BehaviorComponents
{
    public sealed class BoundsPlayerBehaviorComponent2D : PlayerBehaviorComponent2D
    {
        [SerializeField]
        [ReadOnly]
        private Vector2 _viewportSize;

        [SerializeField]
        [ReadOnly]
        private Vector2 _ownerHalfSize;

        [SerializeField]
        [ReadOnly]
        private Vector3 _lastVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector2 _lastPosition;

        public void Initialize(GameData gameData)
        {
            Assert.IsTrue(Behavior.IsKinematic);

            float aspectRatio = Screen.width / (float)Screen.height;
            _viewportSize = new Vector2(gameData.ViewportSize * aspectRatio, gameData.ViewportSize);
            _ownerHalfSize = new Vector2(Behavior.Owner.Radius, Behavior.Owner.Height / 2.0f);
        }

        public override bool OnPhysicsUpdate(float dt)
        {
            _lastVelocity = PlayerBehavior.MoveDirection * PlayerBehavior.PlayerBehaviorData.MoveSpeed;
            _lastPosition = Behavior.Position + _lastVelocity * dt;

            // x-bounds
            if(_lastPosition.x + _ownerHalfSize.x > _viewportSize.x) {
                _lastPosition.x = _viewportSize.x - _ownerHalfSize.x;
            } else if(_lastPosition.x - _ownerHalfSize.x < -_viewportSize.x) {
                _lastPosition.x = -_viewportSize.x + _ownerHalfSize.x;
            }

            // y-bounds
            if(_lastPosition.y + _ownerHalfSize.y > _viewportSize.y) {
                _lastPosition.y = _viewportSize.y - _ownerHalfSize.y;
            } else if(_lastPosition.y - _ownerHalfSize.y < -_viewportSize.y) {
                _lastPosition.y = -_viewportSize.y + _ownerHalfSize.y;
            }

            Behavior.Teleport(_lastPosition);

            return true;
        }
    }
}
