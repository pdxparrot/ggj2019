using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

public class NPCEnemy : NPCBase
{
    [SerializeField]
    private EffectTrigger _deathEffectTrigger;

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
        Model.SetActive(false);
        _deathEffectTrigger.Trigger(() => {
            Destroy(gameObject);
        });
    }
}
