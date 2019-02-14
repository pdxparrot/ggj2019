using UnityEngine;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class AnimationEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _animationTriggerParameter;

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableAnimation) {
                return;
            }

            _animator.SetTrigger(_animationTriggerParameter);
        }
    }
}
