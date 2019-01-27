using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

public class NPCEnemy : NPCBase
{
    [SerializeField]
    private EffectTrigger _deathEffectTrigger;

    [SerializeField]
    [ReadOnly]
    private bool _isDead;

    public bool IsDead => _isDead;

#region Unity Lifecycle
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if(null == player) {
            return;
        }

        // TODO: player score or something?

        Kill();
    }
#endregion

    private void Kill()
    {
        _isDead = true;

        Model.SetActive(false);
        _deathEffectTrigger.Trigger(() => {
            Destroy(gameObject);
        });
    }
}
