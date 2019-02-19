#pragma warning disable 0618    // disable obsolete warning for now

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Effects.EffectTriggerComponents;
using pdxpartyparrot.Game.State;

using Spine;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ggj2019.State
{
    public sealed class GameOverState : Game.State.GameOverState
    {
#region Effects
        [SerializeField]
        private EffectTrigger _gameOverEffect;
#endregion

#region Animation
        [SerializeField]
        private SpineAnimationHelper _gameOverAnimationHelper;

        [SerializeField]
        private string _gameOverEntranceAnimation = "game_over_entrance";

        [SerializeField]
        private string _gameOverAnimation = "game_over_entrance2";
#endregion

        public override void OnEnter()
        {
            base.OnEnter();

            if(NetworkClient.active) {
                ViewerShakeEffectTriggerComponent viewerShakeEffect = _gameOverEffect.GetEffectTriggerComponent<ViewerShakeEffectTriggerComponent>();
                viewerShakeEffect.Viewer = GameManager.Instance.Viewer;

                _gameOverEffect.Trigger();

                TrackEntry track = _gameOverAnimationHelper.SetAnimation(_gameOverEntranceAnimation, false);
                track.Complete += t => {
                    _gameOverAnimationHelper.SetAnimation(_gameOverAnimation, true);
                };
            }
        }
    }
}
