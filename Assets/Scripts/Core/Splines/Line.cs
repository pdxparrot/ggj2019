using UnityEngine;
using UnityEngine.Serialization;

// https://catlikecoding.com/unity/tutorials/curves-and-splines/

// TODO: move to Core.Math
namespace pdxpartyparrot.Core.Splines
{
    public class Line : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("p0")]
        private Vector3 _p0;

        public Vector3 P0
        {
            get => _p0;
            set => _p0 = value;
        }

        [SerializeField]
        [FormerlySerializedAs("p1")]
        private Vector3 _p1;

        public Vector3 P1
        {
            get => _p1;
            set => _p1 = value;
        }
    }
}