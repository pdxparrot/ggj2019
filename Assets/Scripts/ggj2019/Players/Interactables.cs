using System.Collections.Generic;

using pdxpartyparrot.ggj2019.NPCs;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    // TODO: make this core
    [RequireComponent(typeof(Collider2D))]
    public class Interactables : MonoBehaviour
    {
        private Collider2D _trigger;

        private readonly List<NPCBee> _bees = new List<NPCBee>();


#region Unity Life Cycle
        private void Start()
        {
            _trigger = GetComponent<Collider2D>();

            _trigger.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            NPCBee npcBee = other.GetComponent<NPCBee>();
            if(npcBee != null && !npcBee.IsInSwarm) {
                _bees.Add(npcBee);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            NPCBee npcBee = other.GetComponent<NPCBee>();
            if (npcBee != null) {
                _bees.Remove(npcBee);
            }
        }
#endregion

        public NPCBee GetBee()
        {
            if(_bees.Count < 1) {
                return null;
            }

            foreach(NPCBee bee in _bees) {
                if(bee.CanJoinSwarm) {
                    return bee;
                }
            }

            return null;
        }
    }
}
