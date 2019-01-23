using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using UnityEngine;

namespace pdxpartyparrot.Game.Effects
{
    // TODO: rename to WorldZoneEffect
    public sealed class WeatherZoneEffect : MonoBehaviour
    {
        private readonly HashSet<WeatherZone> _zones = new HashSet<WeatherZone>();

        [SerializeField]
        [ReadOnly]
        private Transform _particleSystemParent;

        public Transform ParticleSystemParent
        {
            get => _particleSystemParent;
            set => _particleSystemParent = value;
        }

#region Unity Lifecycle
        private void OnDestroy()
        {
            foreach(WeatherZone zone in _zones) {
                zone.Exit(gameObject);
            }
            _zones.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            weather.Enter(gameObject, ParticleSystemParent);
            _zones.Add(weather);
        }

        private void OnTriggerExit(Collider other)
        {
            WeatherZone weather = other.GetComponent<WeatherZone>();
            if(null == weather) {
                return;
            }

            weather.Exit(gameObject);
            _zones.Remove(weather);
        }
#endregion
    }
}
