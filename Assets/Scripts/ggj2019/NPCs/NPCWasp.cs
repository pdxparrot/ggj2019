using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using pdxpartyparrot.ggj2019.Players;

public class NPCWasp : NPCEnemy
{
    [SerializeField] private float MaxVel;
    [SerializeField] private float Accel;
    [SerializeField] private float pushback;
   
    private Vector3 _accel;
    private Vector3 _velocity;

    private void Start() {
        Pool.Add(this);
        float dir = (transform.position.x > 0) ? -1 : 1;
        _accel = new Vector3(1,0,0) * Accel * dir;
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }

    void Update() {
        if(IsDead) {
            return;
        }

        _velocity += _accel * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);
        transform.position += _velocity * Time.deltaTime;

        var hive = Hive.Nearest(transform.position);
        if(hive.Collides(transform.position)) {
            if(hive.TakeDamage(transform.position))
                Die();
            else
                PushBack();
        }
    }

    void PushBack() {
        transform.position -= _velocity.normalized * pushback;
        _velocity = new Vector3();
    }

    void Die() {
        // TODO effects
        Destroy(gameObject, 0.01f);
    }

    public static ProxPool<NPCWasp> Pool = new ProxPool<NPCWasp>();

    public static NPCWasp Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCWasp;
    }
}
