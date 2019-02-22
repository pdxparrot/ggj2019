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

        [SerializeField]
        private int _skin;

        public int Skin => _skin;

        [SerializeField]
        private string _animation;

        public string Animation => _animation;
    }
}
