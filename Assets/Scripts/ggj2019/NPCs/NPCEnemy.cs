using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

public class NPCEnemy : NPCBase
{
    [SerializeField]
    private int _damage = 1;

#region Unity Lifecycle
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamagePlayer(other.gameObject.GetComponent<Player>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DamagePlayer(other.gameObject.GetComponent<Player>());
    }
#endregion

    private void DamagePlayer(Player player)
    {
        if(IsDead) {
            return;
        }

        if(null == player) {
            return;
        }

        if(player.Damage(_damage)) {
            Kill();
        }
    }
}
