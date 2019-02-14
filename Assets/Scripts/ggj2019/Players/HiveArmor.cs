using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class HiveArmor : MonoBehaviour
    {
        [SerializeField]
        private EffectTrigger _damageEffectTrigger;

        [SerializeField]
        private EffectTrigger _destroyEffectTrigger;

        [SerializeField]
        [ReadOnly]
        private int _health;

        public int Health => _health;

        private SpriteRenderer _spriteRenderer;

#region Unity Lifecycle
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
#endregion

        public void Initialize()
        {
            _health = GameManager.Instance.GameGameData.HiveArmorHealth;
        }

        public bool Damage(int amount)
        {
            _health -= amount;
            if(Health <= 0) {
                ShowDestroy();
                GameManager.Instance.HiveDamage();
                return true;
            }

            ShowDamage();
            return false;
        }

        private void ShowDamage()
        {
            float f = Health / (float)GameManager.Instance.GameGameData.HiveArmorHealth;
            _spriteRenderer.color = new Color(1.0f, f, f);

            _damageEffectTrigger.Trigger();
        }

        private void ShowDestroy()
        {
            _spriteRenderer.enabled = false;
            _destroyEffectTrigger.Trigger();
        }
    }
}
