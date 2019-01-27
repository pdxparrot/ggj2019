using pdxpartyparrot.Core;
using pdxpartyparrot.ggj2019;
using UnityEngine;

using pdxpartyparrot.ggj2019.Players;

public class NPCWasp : NPCEnemy
{
    [SerializeField] private float MaxVel;
    [SerializeField] private float Accel;
    [SerializeField] private float pushback;
    [SerializeField] private float pushbackvel;
   
    private Vector3 _accel;
    private Vector3 _velocity;
    private bool _cooldown;

    private void Start() {
        Pool.Add(this);
        float dir = (transform.position.x > 0) ? -1 : 1;
        _accel = new Vector3(1,0,0) * Accel * dir;
        if(transform.position.x < 0.0f)
            transform.localScale = new Vector3(
                transform.localScale.x * -1.0f,
                transform.localScale.y,
                transform.localScale.z);
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }

    void Update() {
        if(IsDead || GameManager.Instance.IsGameOver  || PartyParrotManager.Instance.IsPaused) {
            return;
        }

        // wait for physics to catch up
        if(_cooldown) {
            return;
        }

        _velocity += _accel * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);
        transform.position += _velocity * Time.deltaTime;

        var hive = Hive.Nearest(transform.position);
        if(hive.Collides(this)) {
            if(hive.TakeDamage(transform.position))
                Kill();
            else
                PushBack();
        }
    }

    private void FixedUpdate() {
        // this is hacky because PushBack() is a teleport
        if(_cooldown) {
            _cooldown = false;
        }
    }

    void PushBack() {
        transform.position -= _velocity.normalized * pushback;
        _velocity = _velocity.normalized * pushbackvel;

        _cooldown = true;
    }

    public static ProxPool<NPCWasp> Pool = new ProxPool<NPCWasp>();

    public static NPCWasp Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCWasp;
    }
}
