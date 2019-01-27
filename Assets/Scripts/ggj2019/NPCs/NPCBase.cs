using pdxpartyparrot.Core.Actors;

public class NPCBase : PhysicsActor2D
{
    public override float Height => Collider.bounds.size.y;
    public override float Radius => Collider.bounds.size.x;

    public override bool IsLocalActor => true;

    public override void OnSpawn() { }
    public override void OnReSpawn() { }
}
