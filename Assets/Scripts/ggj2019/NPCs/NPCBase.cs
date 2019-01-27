using pdxpartyparrot.Core.Actors;
using UnityEngine;

public class NPCBase : PhysicsActor2D
{
    public override float Height => Collider.bounds.size.y;
    public override float Radius => Collider.bounds.size.x;

    public override bool IsLocalActor => true;

    public override void OnSpawn() { }
    public override void OnReSpawn() { }

    public bool Collides(Vector3 pos) {
        Vector3 offset = pos - transform.position;
        return (Vector3.Magnitude(offset) < Radius);
    }

}
