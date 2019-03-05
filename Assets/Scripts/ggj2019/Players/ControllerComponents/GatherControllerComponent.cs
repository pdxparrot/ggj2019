namespace pdxpartyparrot.ggj2019.Players.ControllerComponents
{
    public sealed class GatherControllerComponent : GamePlayerControllerComponent
    {
#region Actions
        public class GatherAction : CharacterActorControllerAction
        {
            public static GatherAction Default = new GatherAction();
        }
#endregion

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(!(action is GatherAction)) {
                return false;
            }

            GamePlayer.DoGather();

            return true;
        }
    }
}
