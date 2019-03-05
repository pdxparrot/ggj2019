namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class JumpControllerComponent3D : CharacterActorControllerComponent3D
    {
#region Actions
        public class JumpAction : CharacterActorControllerAction
        {
            public static JumpAction Default = new JumpAction();
        }
#endregion

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is JumpAction)) {
                return false;
            }

            if(!Behavior.IsGrounded || Behavior.IsSliding) {
                return false;
            }

            Behavior.Jump(Behavior.ControllerData.JumpHeight, Behavior.ControllerData.JumpParam);

            return true;
        }
    }
}
