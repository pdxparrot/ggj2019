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

            // TODO: copied from DefaultPhysicsMove() and probably should be smarter than that
            Vector2 velocity = axes * speed;
            if(!Controller2D.Rigidbody.isKinematic) {
                velocity.y = Controller2D.Rigidbody.velocity.y;
            }

            Vector2 updatedPosition = Controller2D.Rigidbody.position + velocity * dt;
            if(updatedPosition.x > gameSize.x) {
                updatedPosition.x = gameSize.x;
            } else if(updatedPosition.x < -gameSize.x) {
                updatedPosition.x = -gameSize.x;
            }

            if(updatedPosition.y > gameSize.y) {
                updatedPosition.y = gameSize.y;
            } else if(updatedPosition.y < -gameSize.y) {
                updatedPosition.y = -gameSize.y;
            }

            Controller2D.Rigidbody.MovePosition(updatedPosition);

            return true;
        }
    }
}
