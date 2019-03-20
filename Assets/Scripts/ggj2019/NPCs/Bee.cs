using System;

using pdxpartyparrot.Core.Animation;
using pdxpartyparrot.Core.Data;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.Interactables;
using pdxpartyparrot.Game.Swarm;
using pdxpartyparrot.ggj2019.Data;

using TMPro;

using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.ggj2019.NPCs
{
    [RequireComponent(typeof(PooledObject))]
    [RequireComponent(typeof(SpineAnimationHelper))]
    [RequireComponent(typeof(SpineSkinHelper))]
    public sealed class Bee : NPC, ISwarmable, IInteractable
    {
#region ISwarmable
        public Transform Transform => transform;

        public bool CanJoinSwarm => BeeBehavior.IsIdle;

        public bool IsInSwarm => BeeBehavior.HasTargetSwarm;
#endregion

#region IInteractable
        public bool CanInteract => CanJoinSwarm;
#endregion

        [SerializeField]
        private TextMeshPro _gatherText;

        private BeeBehavior BeeBehavior => (BeeBehavior)GameNPCBehavior;

        private SpineSkinHelper _skinHelper;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsTrue(NPCBehavior is BeeBehavior);

            _skinHelper = GetComponent<SpineSkinHelper>();
        }
#endregion

        public override void Initialize(Guid id, ActorBehaviorData data)
        {
            Assert.IsTrue(data is BeeData);

            base.Initialize(id, data);
        }

        public void EnableGatherText(bool enable)
        {
            _gatherText.gameObject.SetActive(enable);
        }

#region Skin
        public void SetDefaultSkin()
        {
            _skinHelper.SetSkin(0);
        }

        public void SetPlayerSkin(Players.Player player)
        {
            _skinHelper.SetSkin(player.SkinIndex);
        }
#endregion

#region ISwarmable
        public void JoinSwarm(Swarm swarm, float radius)
        {
            if(!CanJoinSwarm) {
                return;
            }

            BeeBehavior.OnJoinSwarm(swarm, radius);
        }

        public void RemoveFromSwarm()
        {
            BeeBehavior.OnRemoveFromSwarm();
        }
#endregion
    }
}
