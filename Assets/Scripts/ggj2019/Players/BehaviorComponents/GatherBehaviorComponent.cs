namespace pdxpartyparrot.ggj2019.Players.BehaviorComponents
{
    public sealed class GatherBehaviorComponent : GamePlayerBehaviorComponent
    {
#region Actions
        public class GatherAction : CharacterBehaviorAction
        {
            public static GatherAction Default = new GatherAction();
        }
#endregion

        public override bool OnPerformed(CharacterBehaviorAction action)
        {
            if(!(action is GatherAction)) {
                return false;
            }

            GamePlayer.DoGather();

            return true;
        }
    }
}
