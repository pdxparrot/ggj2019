using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.State
{
    public abstract class GameOverState : SubGameState
    {
        [SerializeField]
        private float _completeWaitTimeSeconds = 5.0f;

        [SerializeField]
        [ReadOnly]
        private Timer _completeTimer;

        [SerializeField]
        private AudioClip _endGameMusic;

        public override void OnEnter()
        {
            base.OnEnter();

            AudioManager.Instance.StopAllMusic();
            AudioManager.Instance.PlayMusic(_endGameMusic);

            _completeTimer.Start(_completeWaitTimeSeconds, () => {
                GameStateManager.Instance.TransitionToInitialState();
            });
        }

        public sealed override void OnExit()
        {
            // NOTE: be careful doing game-related cleanup in here
            // because if the game is restarted from the pause menu
            // it won't get cleaned up

            base.OnExit();
        }

        public override void OnUpdate(float dt)
        {
            _completeTimer.Update(dt);
        }

        public virtual void Initialize()
        {
        }
    }
}
