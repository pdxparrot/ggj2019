using System;
using System.Collections.Generic;

using pdxpartyparrot.Core.Data.Scripting.Nodes;

using UnityEngine;

namespace pdxpartyparrot.Core.Data.Scripting
{
    [CreateAssetMenu(fileName="ScriptData", menuName="pdxpartyparrot/Core/Data/Script Data")]
    [Serializable]
    public sealed class ScriptData : ScriptableObject
    {
        // TODO: this might need to be an array
        [SerializeField]
        private readonly List<ScriptNodeData> _nodes = new List<ScriptNodeData>();

        public IReadOnlyCollection<ScriptNodeData> Nodes => _nodes;
    }
}
