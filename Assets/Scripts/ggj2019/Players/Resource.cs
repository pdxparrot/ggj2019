using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Resource : NPCBase
    {
        public override float Height => Collider.bounds.size.y;
        public override float Radius => Collider.bounds.size.x;

        public override bool IsLocalActor => true;

        public override void OnSpawn() { }
        public override void OnReSpawn() { }
    }
}
