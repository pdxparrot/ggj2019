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
        [ReadOnly]
        private int _health;

        public int Health => _health;

        public void Initialize()
        {
            _health = GameManager.Instance.GameGameData.HiveArmorHealth;
        }

        public bool Damage(int amount)
        {
            _health -= amount;
            if(Health <= 0) {
                ShowDestroy();
                return true;
            }

            ShowDamage();
            return false;
        }

        private void ShowDamage()
        {
            float f = Health / (float)GameManager.Instance.GameGameData.HiveArmorHealth;
            _model.color = new Color(1.0f, f, f);

            _damageEffectTrigger.Trigger();
        }

        private void ShowDestroy()
        {
            _destroyEffectTrigger.Trigger();
        }
    }
}
