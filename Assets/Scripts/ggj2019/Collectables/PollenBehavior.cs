using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.State;
using pdxpartyparrot.ggj2019.Data;
using pdxpartyparrot.ggj2019.Home;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Collectables
{
    public sealed class PollenBehavior : ActorBehavior2D
    {
        private enum State
        {
            Floating,
            FollowingPlayer,
            GoingToHive,
            Collected
        }

        [Space(10)]

        [Header("Pollen")]

#region State
        [SerializeField]
        [ReadOnly]
        private State _state = State.Floating;

        public bool IsFloating => _state == State.Floating;
#endregion

        [Space(10)]

#region Effects
        [SerializeField]
        private EffectTrigger _pickupEffect;

        [SerializeField]
        private EffectTrigger _collectEffect;

        [SerializeField]
        private Oscillator _oscillator;
#endregion

        [Space(10)]

        [SerializeField]
        [ReadOnly]
        private Players.Player _followPlayer;

        [SerializeField]
        [ReadOnly]
        private Hive _hive;

        private Pollen Pollen => (Pollen)Owner;

        public PollenBehaviorData PollenBehaviorData => (PollenBehaviorData)BehaviorData;

        public override void Think(float dt)
        {
            if(GameManager.Instance.IsGameOver) {
                return;
            }

            base.Think(dt);

            switch(_state)
            {
            case State.FollowingPlayer:
                if(_followPlayer.IsDead) {
                    SetState(State.Floating);
                }
                break;
            }
        }

        private void SetState(State state)
        {
            _state = state;
            switch(_state)
            {
            case State.Floating:
                Pollen.EnableCollectText(true);
                _oscillator.enabled = true;
                _followPlayer = null;
                _hive = null;
                break;
            case State.FollowingPlayer:
                Pollen.EnableCollectText(false);
                _oscillator.enabled = false;
                break;
            case State.GoingToHive:
                Pollen.EnableCollectText(false);
                _oscillator.enabled = false;
                break;
            case State.Collected:
                Pollen.EnableCollectText(false);
                _oscillator.enabled = false;
                _followPlayer = null;
                _hive = null;
                break;
            }
        }

        protected override void PhysicsUpdate(float dt)
        {
            base.PhysicsUpdate(dt);

            if(!CanMove) {
                return;
            }

            switch(_state)
            {
            case State.Floating:
                Float(dt);
                break;
            case State.FollowingPlayer:
                FollowPlayer(dt);
                break;
            case State.GoingToHive:
                GoToHive(dt);
                break;
            }
        }

        private void Float(float dt)
        {
            // recycle if we're off the screen
            Vector3 position = transform.position;
            if(position.y - Pollen.Height / 2.0f > GameStateManager.Instance.GameManager.GameData.ViewportSize) {
                Pollen.Recycle();
                return;
            }

            position.y += PollenBehaviorData.FloatSpeed * dt;
            Movement2D.Teleport(position);
        }

        private void FollowPlayer(float dt)
        {
            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, _followPlayer.PollenTarget.position, PollenBehaviorData.FollowPlayerSpeed * dt);
            Movement2D.Teleport(position);
        }

        private void GoToHive(float dt)
        {
            Vector3 position = transform.position;
            position = Vector3.MoveTowards(position, _hive.transform.position, PollenBehaviorData.GoToHiveSpeed * dt);
            Movement2D.Teleport(position);
        }

#region Actions
        private void Gather(Players.Player player)
        {
            if(!Pollen.CanBeCollected || null == player || !player.CanGather) {
                return;
            }

            player.AddPollen(Pollen);
            _pickupEffect.Trigger();

            _followPlayer = player;
            SetState(State.FollowingPlayer);
        }

        private void Collect(Hive hive)
        {
            if(null == hive || _state != State.GoingToHive) {
                return;
            }

            hive.CollectPollen(_followPlayer);

            SetState(State.Collected);

            _collectEffect.Trigger(() => {
                Pollen.Recycle();
            });
        }
#endregion

#region Events
        public void OnUnload(Hive hive)
        {
            _hive = hive;

            SetState(State.GoingToHive);
        }

        public override void OnSpawn(SpawnPoint spawnpoint)
        {
            base.OnSpawn(spawnpoint);

            SetState(State.Floating);
        }

        public override void TriggerEnter(GameObject triggerObject)
        {
            Gather(triggerObject.GetComponent<Players.Player>());
            Collect(triggerObject.GetComponent<Hive>());
        }

        public override void TriggerStay(GameObject triggerObject)
        {
            Collect(triggerObject.GetComponent<Hive>());
        }

       public override void TriggerExit(GameObject triggerObject)
        {
            Gather(triggerObject.GetComponent<Players.Player>());
            Collect(triggerObject.GetComponent<Hive>());
        }
#endregion
    }
}
