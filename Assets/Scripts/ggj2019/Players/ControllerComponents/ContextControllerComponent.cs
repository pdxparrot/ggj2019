namespace pdxpartyparrot.ggj2019.Players.ControllerComponents
{
    public sealed class ContextControllerComponent : GamePlayerControllerComponent
    {
#region Actions
        public class ContextAction : CharacterBehaviorAction
        {
            public static ContextAction Default = new ContextAction();
        }
#endregion

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is ContextAction)) {
                return false;
            }

            //GamePlayer.DoContext();

            return true;
        }
    }
}
