using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs.Control
{
    // TODO: make this core
    public class Swarm : MonoBehaviour
    {
        [SerializeField]
        private float _swarmRadius;

        [SerializeField]
        private bool isPlayerSwarm;

        private readonly List<ISwarmable> _swarmables = new List<ISwarmable>();

        private readonly List<Vector3> _swarmOffset = new List<Vector3>();

        public bool HasSwarm => _swarmables.Count > 0;

        public void Add(ISwarmable swarmable)
        {
            _swarmables.Add(swarmable);

            if(isPlayerSwarm && swarmable.CanJoinSwarm) {
                swarmable.JoinSwarm(this, _swarmRadius);
            }
        }

        public bool DoContext()
        {
            if(!HasSwarm) {
                return false;
            }

            if(_swarmables[0].DoContext()) {
                _swarmables.RemoveAt(0);
                return true;
            }
            return false;
        }

        public int Kill(int amount)
        {
            int killed = 0;
            for(int i=0; i<_swarmables.Count && killed < amount; ++i) {
                _swarmables[i].Kill();
                killed++;
            }
            _swarmables.RemoveRange(0, killed);

            return killed;
        }
    }
}
