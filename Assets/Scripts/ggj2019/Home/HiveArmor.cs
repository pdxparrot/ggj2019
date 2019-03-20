using System.Collections.Generic;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Home
{
    public sealed class HiveArmor : MonoBehaviour
    {
#region Effects
        [SerializeField]
        private EffectTrigger _damageEffectTrigger;

        [SerializeField]
        private EffectTrigger _destroyEffectTrigger;
#endregion

        [SerializeField]
        private SpriteRenderer _model;

        [SerializeField]
        private Hive _owner;

#region Neighbors
        [SerializeField]
        private HiveArmor _acrossNeighbor;

        [SerializeField]
        private HiveArmor _upNeighbor;

        [SerializeField]
        private HiveArmor _downNeighbor;
#endregion

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        private readonly Queue<HiveArmor> _damageQueue = new Queue<HiveArmor>();
        private readonly HashSet<HiveArmor> _damageVisited = new HashSet<HiveArmor>();

        public void Initialize()
        {
            _health = _owner.HiveBehavior.HiveBehaviorData.HiveArmorHealth;
        }

        public bool Damage()
        {
            if(_owner.Immune) {
                return false;
            }

            // try to damage ourselves first
            if(Health > 0) {
                _health--;
                if(Health <= 0) {
                    DestroyArmor();
                    _owner.ArmorDestroyed();
                    return true;
                }

                DamageArmor();
                return false;
            }

            // didn't damage ourself, so BFS damage our neighbors

            _damageVisited.Clear();
            _damageVisited.Add(this);

            _damageQueue.Clear();
            _damageQueue.Enqueue(_acrossNeighbor);

            if(null != _upNeighbor) {
                _damageQueue.Enqueue(_upNeighbor);
            }

            if(null != _downNeighbor) {
                _damageQueue.Enqueue(_downNeighbor);
            }

            while(_damageQueue.Count > 0) {
                HiveArmor armor = _damageQueue.Dequeue();
                if(armor.Health > 0) {
                    return armor.Damage();
                }

                _damageVisited.Add(armor);

                // we don't need to come back across because we'll have hit this by traversing our up / down neighbors

                if(armor._upNeighbor != null && !_damageVisited.Contains(armor._upNeighbor)) {
                    _damageQueue.Enqueue(armor._upNeighbor);
                }

                if(armor._downNeighbor != null && !_damageVisited.Contains(armor._downNeighbor)) {
                    _damageQueue.Enqueue(armor._downNeighbor);
                }
            }
    
            // this can happen if something tries to damage the hive
            // when it has no armor left, so we let it slide with a warning
            Debug.LogWarning("Unable to damage any armor!");
            return false;
        }

        private void DamageArmor()
        {
            float f = Health / (float)_owner.HiveBehavior.HiveBehaviorData.HiveArmorHealth;
            _model.color = new Color(1.0f, f, f);

            _damageEffectTrigger.Trigger();
        }

        private void DestroyArmor()
        {
            _destroyEffectTrigger.Trigger();
        }
    }
}
