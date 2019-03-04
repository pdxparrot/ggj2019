using pdxpartyparrot.Core.Audio;
using pdxpartyparrot.Core.Time;
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
                GameStateManager.Instance.TransitionToInitialStateAsync();
            });
        }

        public override void OnExit()
        {
            AudioManager.Instance.StopAllMusic();

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
