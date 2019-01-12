namespace pdxpartyparrot.Core.Scripting.Nodes
{
    public sealed class End : ScriptNode
    {
        [Input]
        ScriptNode _prev;

        public override void Run(ScriptContext context)
        {
            context.Complete();
        }
    }
}
