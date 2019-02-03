using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public abstract class NPCEnemy : NPCBase
    {
        [SerializeField]
        private int _damage = 1;

        public int Damage => _damage;

#region Unity Lifecycle
        private void OnTriggerEnter2D(Collider2D other)
        {
            DamagePlayer(other.gameObject.GetComponent<Players.Player>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            DamagePlayer(other.gameObject.GetComponent<Players.Player>());
        }
#endregion

        private void DamagePlayer(Players.Player player)
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
}
