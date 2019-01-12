namespace pdxpartyparrot.Core.Scripting.Nodes
{
    public sealed class Start : ScriptNode
    {
        [Output]
        ScriptNode _next;

        public override void Run(ScriptContext context)
        {
            context.Advance();
        }
    }
}
