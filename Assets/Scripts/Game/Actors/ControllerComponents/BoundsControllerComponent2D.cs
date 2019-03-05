using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class BoundsControllerComponent2D : CharacterActorControllerComponent2D
    {
        public override bool OnPhysicsMove(Vector3 axes, float speed, float dt)
        {
            // TODO: we probably don't need to calculate this every frame...
            float aspectRatio = (Screen.width / (float)Screen.height);
            Vector2 gameSize = new Vector2(GameStateManager.Instance.GameManager.GameData.GameSize2D * aspectRatio,
                                           GameStateManager.Instance.GameManager.GameData.GameSize2D);
            Vector2 halfSize = new Vector2(Behavior.Owner.Radius, Behavior.Owner.Height / 2.0f);

            // TODO: this was originally copied from DefaultPhysicsMove() and probably should be smarter than that
            // TODO: this should have acceleration and momentum

            Vector3 velocity = axes * speed;
            if(!Behavior.IsKinematic) {
                velocity.y = Behavior.Velocity.y;
            }

            Vector2 updatedPosition = Behavior.Position + velocity * dt;
            if(updatedPosition.x + halfSize.x > gameSize.x) {
                updatedPosition.x = gameSize.x - halfSize.x;
            } else if(updatedPosition.x - halfSize.x < -gameSize.x) {
                updatedPosition.x = -gameSize.x + halfSize.x;
            }

            if(updatedPosition.y + halfSize.y > gameSize.y) {
                updatedPosition.y = gameSize.y - halfSize.y;
            } else if(updatedPosition.y - halfSize.y < -gameSize.y) {
                updatedPosition.y = -gameSize.y + halfSize.y;
            }

            Behavior.MovePosition(updatedPosition);

            return true;
        }
    }
}
