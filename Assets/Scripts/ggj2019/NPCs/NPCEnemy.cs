using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

public class NPCEnemy : NPCBase
{
    [SerializeField]
    private EffectTrigger _deathEffectTrigger;

    [SerializeField] private int _damage = 1;

    [SerializeField]
    [ReadOnly]
    private bool _isDead;

    public bool IsDead => _isDead;

#region Unity Lifecycle
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (_isDead) {
            return;
        }

        Player player = other.gameObject.GetComponent<Player>();
        if(null == player) {
            return;
        }

        Debug.Log(gameObject.name);

        if (player.Damage(_damage)) {
            Kill();
        }
    }
#endregion

    protected void Kill()
    {
        _isDead = true;

        Model.SetActive(false);
        _deathEffectTrigger.Trigger(() => {
            Destroy(gameObject);
        });
    }
}
