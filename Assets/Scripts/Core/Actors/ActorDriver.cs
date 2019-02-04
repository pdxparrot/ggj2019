using UnityEngine;
using UnityEngine.Serialization;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class ActorDriver : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("_controller")]
        private ActorController _behavior;

        protected ActorController Behavior => _behavior;

        protected virtual bool CanDrive => !PartyParrotManager.Instance.IsPaused;
    }
}
