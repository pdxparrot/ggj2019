using pdxpartyparrot.Core.Actors;
using Spine.Unity;
using UnityEngine;

public class NPCBase : PhysicsActor2D
{
    public override float Height => Collider.bounds.size.y / 2.0f;
    public override float Radius => Collider.bounds.size.x / 2.0f;

    public override bool IsLocalActor => true;

    public override void OnSpawn() { }
    public override void OnReSpawn() { }

    public bool Collides(Actor other, float distance = 0.0f) {
        Vector3 offset = other.transform.position - transform.position;
        float r = other.Radius + Radius;
        return Vector3.Magnitude(offset) < r + distance;
    }

    [SerializeField]
    protected SkeletonAnimation _animation;

    protected void SetAnimation(string animationName, bool loop)
    {
        _animation.AnimationState.SetAnimation(0, animationName, loop);
    }

}
