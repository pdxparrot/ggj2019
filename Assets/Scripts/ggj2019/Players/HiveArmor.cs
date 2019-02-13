using DG.Tweening;

using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class HiveArmor : MonoBehaviour
    {
        [SerializeField]
        private ShakeConfig _damageShakeConfig = new ShakeConfig(0.3f, 0.3f, 20, 130.0f);

        [SerializeField]
        private ShakeConfig _destroyShakeConfig = new ShakeConfig(0.1f, 0.3f, 20, 130.0f);

        [SerializeField]
        private EffectTrigger _damageEffectTrigger;

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

            transform.DOShakePosition(_damageShakeConfig.Duration, _damageShakeConfig.Strength, _damageShakeConfig.Vibrato, _damageShakeConfig.Randomness);

            _damageEffectTrigger.Trigger();
        }

        private void ShowDestroy()
        {
            transform.DOShakePosition(_destroyShakeConfig.Duration, _destroyShakeConfig.Strength, _destroyShakeConfig.Vibrato, _destroyShakeConfig.Randomness)
                .OnComplete(() => _spriteRenderer.enabled = false);

            _damageEffectTrigger.Trigger(() => gameObject.SetActive(false));
        }
    }
}
