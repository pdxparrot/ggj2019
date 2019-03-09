namespace pdxpartyparrot.ggj2019.Players.BehaviorComponents
{
    public sealed class ContextBehaviorComponent : GamePlayerBehaviorComponent
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
