using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

public class NPCEnemy : NPCBase
{
    [SerializeField] private int _damage = 1;

#region Unity Lifecycle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsDead) {
            return;
        }

        Player player = other.gameObject.GetComponent<Player>();
        if(null == player) {
            return;
        }

        //Debug.Log(gameObject.name);

        if (player.Damage(_damage)) {
            Kill();
        }
    }
#endregion
}
