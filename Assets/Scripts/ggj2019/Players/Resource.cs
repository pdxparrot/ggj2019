using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ggj2019.Players
{
    public sealed class Resource : NPCBase
    {
        public override float Height => Collider.bounds.size.y;
        public override float Radius => Collider.bounds.size.x;

        public override bool IsLocalActor => true;

#region Unity Lifecycle
        private void OnCollisionEnter2D(Collision2D other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(null == player) {
                return;
            }

            player.CollectResource();

            Destroy(gameObject);
        }
#endregion

        public override void OnSpawn() { }
        public override void OnReSpawn() { }
    }
}
