using JetBrains.Annotations;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Data;
using pdxpartyparrot.ggj2019.Data;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    public sealed class NPCBeetle : NPCEnemy
    {
        [SerializeField]
        private EffectTrigger _attackEffect;

        [SerializeField]
        [ReadOnly]
        private NPCFlower _flower;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private SpawnPoint _spawnpoint;

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _harvestCooldownTimer = new Timer();

        private NPCBeetleData BeetleData => (NPCBeetleData)NPCData;

#region Unity Lifecycle
        protected override void Update()
        {
            base.Update();

            float dt = Time.deltaTime;

            _harvestCooldownTimer.Update(dt);
        }
#endregion

#region Spawn
        public override bool OnSpawn(SpawnPoint spawnpoint)
        {
            if(!base.OnSpawn(spawnpoint)) {
                return false;
            }

            if(!spawnpoint.Acquire(this, () => _spawnpoint = null)) {
                Debug.LogError("Unable to acquire spawnpoint!");
                return false;
            }
            _spawnpoint = spawnpoint;

            _flower = _spawnpoint.GetComponentInParent<NPCFlower>();
            Assert.IsFalse(_flower.IsDead);
            _flower.AcquirePollenSpawnpoint(this);

            return true;
        }

        public override void OnDeSpawn()
        {
            _harvestCooldownTimer.Stop();

            _attackEffect.StopTrigger();

            if(_flower != null) {
                _flower.ReleasePollenSpawnpoint();
                _flower = null;
            }

            if(null != _spawnpoint) {
                _spawnpoint.Release();
                _spawnpoint = null;
            }

            base.OnDeSpawn();
        }
#endregion

        public override void Initialize(NPCData data)
        {
            Assert.IsTrue(data is NPCBeetleData);

            base.Initialize(data);

            _harvestCooldownTimer.Start(BeetleData.HarvestCooldown, HarvestFlower);

            SetHarvestAnimation();
        }

#region Animations
        private void SetIdleAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeetleData.IdleAnimation, true);
        }

        private void SetHarvestAnimation()
        {
            _spineAnimationHelper.SetAnimation(BeetleData.HarvestAnimation, true);
        }
#endregion

        private void HarvestFlower()
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            _attackEffect.Trigger();

            _flower.BeetleHarvest(Damage);
            if(_flower.IsDead) {
                _flower = null;

                Kill(false);
                return;
            }

            _harvestCooldownTimer.Start(BeetleData.HarvestCooldown, HarvestFlower);
        }

        public override void Kill(bool playerKill)
        {
            if(playerKill) {
                GameManager.Instance.BeetleKilled();
            }

            base.Kill(playerKill);
        }
    }
}
