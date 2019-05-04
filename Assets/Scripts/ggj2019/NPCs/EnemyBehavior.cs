using pdxpartyparrot.Core.Time;
using pdxpartyparrot.Core.World;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public abstract class EnemyBehavior : NPCBehavior
    {
        private ITimer _immunityTimer;

        protected bool IsImmune => _immunityTimer.IsRunning;

        private Enemy EnemyNPC => (Enemy)NPC;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            _immunityTimer = TimeManager.Instance.AddTimer();
        }

        protected override void OnDestroy()
        {
            if(TimeManager.HasInstance) {
                TimeManager.Instance.RemoveTimer(_immunityTimer);
            }
            _immunityTimer = null;

            base.OnDestroy();
        }
#endregion

        private void DamagePlayer(Players.Player player)
        {
            if(IsDead || GameManager.Instance.IsGameOver || null == player) {
                return;
            }

            player.Damage();
            if(!IsImmune) {
                EnemyNPC.Kill(player);
            }
        }

#region Events
        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            _immunityTimer.Start(GameManager.Instance.GameGameData.EnemySpawnImmunitySeconds);
        }

        public override void TriggerEnter(GameObject collideObject)
        {
            base.TriggerEnter(collideObject);

            DamagePlayer(collideObject.GetComponent<Players.Player>());
        }

        public override void TriggerStay(GameObject collideObject)
        {
            base.TriggerStay(collideObject);

            DamagePlayer(collideObject.GetComponent<Players.Player>());
        }

        public override void TriggerExit(GameObject collideObject)
        {
            base.TriggerExit(collideObject);

            DamagePlayer(collideObject.GetComponent<Players.Player>());
        }
#endregion
    }
}
