using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBeetle : NPCEnemy
{
    public NPCFlower Flower { get; private set; }

    public int Pollen { get; private set; }

    private void Start() {
        Pool.Add(this);
        Flower = NPCFlower.Nearest(transform.position);
        Pollen = 0;
    }

    private void OnDestroy() {
        Pool.Remove(this);
    }

    void Update() {
        if(Flower)
            Pollen += Flower.Harvest();
    }

    public static ProxPool<NPCBeetle> Pool = new ProxPool<NPCBeetle>();

    public static NPCBeetle Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCBeetle;
    }
}
