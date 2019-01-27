using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBeetle : NPCEnemy
{
    public NPCFlower Flower { get; private set; }

    public int Pollen { get; private set; }

    void Start() {
        // TODO find a flower
        Pollen = 0;
    }

    void Update() {
        if(Flower)
            Pollen += Flower.Harvest();
    }
}
