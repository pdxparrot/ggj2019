using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Players;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public abstract class Enemy : NPC2D
    {
        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _immunityTimer = new Timer();

        protected bool IsImmune => _immunityTimer.IsRunning;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Collider.isTrigger = true;
        }

        protected override void Update()
        {
            float dt = Time.deltaTime;

            _immunityTimer.Update(dt);

            base.Update();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DamagePlayer(other.gameObject.GetComponent<Players.Player>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            DamagePlayer(other.gameObject.GetComponent<Players.Player>());
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            _immunityTimer.Start(GameManager.Instance.GameGameData.EnemySpawnImmunitySeconds);

            return true;
        }
#endregion

        private void DamagePlayer(Players.Player player)
        {
            if(IsDead || GameManager.Instance.IsGameOver) {
                return;
            }

            if(null == player) {
                return;
            }

            if(player.Damage() && !IsImmune) {
                Kill(true);
            }
        }
    }
}
