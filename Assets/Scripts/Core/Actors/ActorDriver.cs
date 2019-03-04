using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorDriver : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("_controller")]
        private ActorBehavior _behavior;

        protected ActorBehavior Behavior => _behavior;

        protected virtual bool CanDrive => !PartyParrotManager.Instance.IsPaused;
    }
}
