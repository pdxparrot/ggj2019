using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.NPCs.Control
{
    // TODO: make this core
    public class Swarm : MonoBehaviour
    {
        [SerializeField]
        private float _swarmRadius;

        private readonly List<ISwarmable> _swarmables = new List<ISwarmable>();

        private GameObject _swarmContainer;

        public bool HasSwarm => _swarmables.Count > 0;

#region Unity Lifecycle
        private void Awake()
        {
            _swarmContainer = new GameObject("swarm");
            _swarmContainer.transform.SetParent(transform);
        }

        private void Destroy()
        {
            Destroy(_swarmContainer);
        }
#endregion

        public void Add(ISwarmable swarmable)
        {
            if(!swarmable.CanJoinSwarm) {
                return;
            }

            _swarmables.Add(swarmable);
            swarmable.JoinSwarm(this, _swarmRadius);
            swarmable.Transform.SetParent(_swarmContainer.transform);
        }

        public int Remove(int amount)
        {
            int removed = 0;
            for(int i=0; i<_swarmables.Count && removed < amount; ++i) {
                ISwarmable swarmable = _swarmables[i];

                swarmable.Transform.SetParent(null);
                swarmable.RemoveFromSwarm();

                removed++;
            }
            _swarmables.RemoveRange(0, removed);

            return removed;
        }

        public void RemoveAll()
        {
            foreach(ISwarmable swarmable in _swarmables) {
                swarmable.Transform.SetParent(null);
                swarmable.RemoveFromSwarm();
            }
            _swarmables.Clear();
        }
    }
}
