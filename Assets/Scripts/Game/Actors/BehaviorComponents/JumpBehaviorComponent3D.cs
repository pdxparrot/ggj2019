namespace pdxpartyparrot.Game.Actors.BehaviorComponents
{
    public sealed class JumpBehaviorComponent3D : CharacterBehaviorComponent3D
    {
#region Actions
        public class JumpAction : CharacterBehaviorAction
        {
            public static JumpAction Default = new JumpAction();
        }
#endregion

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is JumpAction)) {
                return false;
            }

            if(!Behavior.IsGrounded || Behavior.IsSliding) {
                return false;
            }

            Behavior.Jump(Behavior.BehaviorData.JumpHeight, Behavior.BehaviorData.JumpParam);

            return true;
        }
    }
}
