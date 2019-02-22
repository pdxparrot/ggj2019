using System.Collections.Generic;

using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core
{
    public class SaveGameManager : SingletonBehavior<SaveGameManager>
    {
        [SerializeField]
        private string _saveFileName;

        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

// TODO: managers should be able to register with this dude to save their data to the dictionary
// tho maybe there's a more structured way to handle this?
// https://docs.unity3d.com/Manual/JSONSerialization.html
    }
}
