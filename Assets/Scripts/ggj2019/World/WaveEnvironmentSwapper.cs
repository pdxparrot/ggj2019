using System;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    [Serializable]
    public class WaveEnvironmentSwapper
    {
        [SerializeField]
        private int _wave;

        public int Wave => _wave;

        // TODO: may as well just make this the skin name instead of an index
        // it'd be a lot easier to work with
        [SerializeField]
        private int _skin;

        public int Skin => _skin;

        [SerializeField]
        private string _animation;

        public string Animation => _animation;

        [SerializeField]
        private bool _fade;

        public bool Fade => _fade;
    }
}
