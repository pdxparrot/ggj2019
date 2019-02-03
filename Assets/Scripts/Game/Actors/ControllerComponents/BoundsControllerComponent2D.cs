using pdxpartyparrot.Game.State;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class BoundsControllerComponent2D : CharacterActorControllerComponent
    {
        private CharacterActorController2D Controller2D => (CharacterActorController2D)Controller;

        public override bool OnPhysicsMove(Vector3 axes, float speed, float dt)
        {
            float aspectRatio = (Screen.width / (float)Screen.height);
            Vector2 gameSize = new Vector2(GameStateManager.Instance.GameManager.GameData.GameSize2D * aspectRatio,
                                           GameStateManager.Instance.GameManager.GameData.GameSize2D);

            Vector2 halfSize = new Vector2(Controller2D.Owner.Radius, Controller2D.Owner.Height / 2.0f);

            // TODO: this was originally copied from DefaultPhysicsMove() and probably should be smarter than that
            // TODO: this should have acceleration and momentum

            Vector2 velocity = axes * speed;
            if(!Controller2D.Rigidbody.isKinematic) {
                velocity.y = Controller2D.Rigidbody.velocity.y;
            }

            Vector2 updatedPosition = Controller2D.Rigidbody.position + velocity * dt;
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

            Controller2D.Rigidbody.MovePosition(updatedPosition);

            return true;
        }
    }
}
