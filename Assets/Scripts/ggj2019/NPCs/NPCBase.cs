using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using Spine.Unity;
using UnityEngine;

public class NPCBase : PhysicsActor2D
{
    public override float Height => Collider.bounds.size.y / 2.0f;
    public override float Radius => Collider.bounds.size.x / 2.0f;

    public override bool IsLocalActor => true;

    [SerializeField]
    [ReadOnly]
    private bool _isDead;

    public bool IsDead
    {
        get => _isDead;
        set => _isDead = value;
    }

    [SerializeField]
    protected EffectTrigger _spawnEffect;

    [SerializeField]
    protected EffectTrigger _deathEffect;

    [SerializeField]
    protected SkeletonAnimation _animation;

    public override void OnSpawn() {
        IsDead = false;

        if(null != _spawnEffect) {
            _spawnEffect.Trigger();
        }
    }

    public override void OnReSpawn() {
        IsDead = false;

        if(null != _spawnEffect) {
            _spawnEffect.Trigger();
        }
    }

    public virtual void Kill() {
        IsDead = true;

        Model.SetActive(false);
        if(null != _deathEffect) {
            _deathEffect.Trigger(() => {
                Destroy(gameObject);
            });
        } else {
            Destroy(gameObject, 0.1f);
        }
    }

    public bool Collides(Actor other, float distance = 0.0f) {
        Vector3 offset = other.transform.position - transform.position;
        float r = other.Radius + Radius;
        return Vector3.Magnitude(offset) < r + distance;
    }

    protected void SetAnimation(string animationName, bool loop)
    {
        _animation.AnimationState.SetAnimation(0, animationName, loop);
    }

}
