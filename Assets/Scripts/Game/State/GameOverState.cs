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

        public override void OnEnter()
        {
            _completeTimer.Start(_completeWaitTimeSeconds, () => {
                GameStateManager.Instance.TransitionToInitialState();
            });
        }

        public override void OnUpdate(float dt)
        {
            _completeTimer.Update(dt);
        }

        public abstract void Initialize();
    }
}
