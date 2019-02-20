using pdxpartyparrot.Core.Splines;
using pdxpartyparrot.Core.World;
using pdxpartyparrot.ggj2019.Home;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.World
{
    [RequireComponent(typeof(SpawnPoint))]
    [RequireComponent(typeof(BezierSpline))]
    public sealed class WaspSpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private HiveArmor _armorToAttack;

        public HiveArmor ArmorToAttack => _armorToAttack;
    }
}
