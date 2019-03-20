namespace pdxpartyparrot.ggj2019.NPCs
{
    public abstract class Enemy : NPC
    {
#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider.isTrigger = true;
        }
#endregion
    }
}
